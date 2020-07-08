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
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.grids = this.node.children;
        _addEvent("ItemMoveEnd",function(node,position,lastPosition){
            for(let i = 0;i < this.grids.length;i++)
            {
                let worldPos = this.grids[i].convertToWorldSpaceAR(cc.v3(0,0,0));
                if(Math.abs(worldPos.x - position.x) <= this.grids[i].width / 2 && Math.abs(worldPos.y - position.y) <= this.grids[0].height / 2)
                {
                    node.position = node.parent.convertToNodeSpaceAR(worldPos);
                    console.log(this.grids[i].name + "->reciveEvent" + ":" + position + ":" + worldPos + ":" + this.grids[i].width + ":" + this.grids[i].height);
                    return;
                }
            }
            node.position = lastPosition;
        }.bind(this),3)
    },

    start () {

    },

    // update (dt) {},
});
