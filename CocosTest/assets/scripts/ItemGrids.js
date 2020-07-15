// Learn cc.Class:
//  - https://docs.cocos.com/creator/manual/en/scripting/class.html
// Learn Attribute:
//  - https://docs.cocos.com/creator/manual/en/scripting/reference/attributes.html
// Learn life-cycle callbacks:
//  - https://docs.cocos.com/creator/manual/en/scripting/life-cycle-callbacks.html

cc.Class({
    extends: cc.Component,

    properties: {
        // foo: {
        //     // ATTRIBUTES:
        //     default: null,        // The default value will be used only when the component attaching
        //                           // to a node for the first time
        //     type: cc.SpriteFrame, // optional, default is typeof default
        //     serializable: true,   // optional, default is true
        // },
        // bar: {
        //     get () {
        //         return this._bar;
        //     },
        //     set (value) {
        //         this._bar = value;
        //     }
        // },
        testSprite : {
            default : null,
            type : cc.Sprite
        }
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.items;
        this.itemPrefab;
        this.itemIcons = [];
        cc.loader.loadResDir("texture",cc.SpriteFrame,function(error,spriteFrames)
        {
            for(let i = 0;i < spriteFrames.length;i++)
            {
                this.itemIcons[i] = spriteFrames[i];
            }
            this.items = new Array(spriteFrames.length);
            console.log("loadImage:" + this.itemIcons.length);
        }.bind(this))
        cc.loader.loadRes("Item",cc.Prefab,function(error,prefab) {
            this.itemPrefab = prefab;
            console.log("loadItem:" + prefab);
        }.bind(this))

        this.grids = [];
        for(let i = 0;i < this.node.children.length;i++)
        {
            this.grids.push({item : null,node : this.node.children[i]});
        }
        
        _addEvent("ItemMoveEnd",function(item,position){
            for(let i = 0;i < this.grids.length;i++)
            {
                let worldPos = this.grids[i].node.convertToWorldSpaceAR(cc.v3(0,0,0));
                if(Math.abs(worldPos.x - position.x) <= this.grids[i].node.width / 2 && Math.abs(worldPos.y - position.y) <= this.grids[0].node.height / 2)
                {
                    if(this.grids[i].item != null)
                    {
                        this.grids[i].item.node.parent = this.grids[item.gridIndex].node;
                        this.grids[i].item.node.position = cc.v3(0,0,0);
                        this.grids[i].item.gridIndex = item.gridIndex;
                        this.grids[item.gridIndex].item = this.grids[i].item;

                        item.node.parent = this.grids[i].node;
                        item.node.position = cc.v3(0,0,0);
                        item.gridIndex = i;
                        this.grids[i].item = item;
                    }
                    else
                    {
                        this.grids[item.gridIndex].item = null;
                        this.grids[i].item = item;
                        item.gridIndex = i;
                        item.node.parent = this.grids[i].node;
                        item.node.position = cc.v3(0,0,0);
                    }
                    return;
                }
            }
            item.node.parent = this.grids[item.gridIndex].node;
            item.node.position = cc.v3(0,0,0);
        }.bind(this),2)

        //cc.systemEvent.on(cc.SystemEvent.EventType.KEY_DOWN,this.onAddItem,this); 
    },

    start () {

    },

    onKeyUp(event)
    {
        console.log(event.keyCode);
    },

    // update (dt) {},
    onAddItem(event)
    {
        let id = Math.floor(Math.random() * this.itemIcons.length);
        if(this.items[id])
        {
            this.items[id].addCount(1);
            return;
        }
        let item = cc.instantiate(this.itemPrefab).getComponent("Item");
        this.items[id] = item;
        item.node.parent = this.node;
        item.itemId = id;
        item.itemIcon.spriteFrame = this.itemIcons[id];
        item.setQuality(Math.floor(Math.random() * 3));
        for(let i = 0;i < this.grids.length;i++)
        {
            if(this.setItemLocation(i,item))
                break;
        }
    },

    setItemLocation(index,item)
    {
        if(this.grids[index].item == null)
        {
            this.grids[index].item = item;
            item.gridIndex = index;
            item.node.parent = this.grids[index].node;
            item.node.position = cc.v3(0,0,0);
            item.addCount(1);
            return true;
        }
        return false;
    }
});
