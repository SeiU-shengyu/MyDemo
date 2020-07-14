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
        this.node.on(cc.Node.EventType.TOUCH_START,this.onTouchStart,this);
        this.node.on(cc.Node.EventType.TOUCH_MOVE,this.onTouchMove,this);
        this.node.on(cc.Node.EventType.TOUCH_END,this.onTouchEnd,this);
        this.node.on(cc.Node.EventType.TOUCH_CANCEL,this.onTouchCancle,this);

        this.itemIcon = this.node.getChildByName("Icon").getComponent(cc.Sprite);
        this.spriteBg = this.node.getChildByName("Bg").getComponent(cc.Sprite);
        this.countsStr = this.node.getChildByName("Counts").getComponent(cc.Label);

        this.gridIndex = 0;
        this.itemId = 0;
        this.counts = 0;
        this.quality = 0;
        this.countsStr.string = this.counts;

        this.itemMoveParent = cc.find("Canvas/Items");
    },

    start () {

    },

    // update (dt) {},

    addCount(count)
    {
        this.counts += count;
        this.countsStr.string = this.counts;
    },
    setQuality(quality)
    {
        this.quality = quality;
        switch(quality)
        {
            case 0:
                this.spriteBg.node.color = cc.Color.WHITE;
                break;
            case 1:
                this.spriteBg.node.color = cc.Color.GREEN;
                break;
            case 2:
                this.spriteBg.node.color = cc.Color.BLUE;
                break;
        }
    },

    onTouchStart(event)
    {
        console.log("Item->touchStart");
        this.node.parent = this.itemMoveParent;
        this.node.position = this.node.parent.convertToNodeSpaceAR(event.getLocation());
    },
    onTouchMove(event)
    {
        console.log("Item->touchMove");
        this.node.position = this.node.parent.convertToNodeSpaceAR(event.getLocation());
    },
    onTouchEnd(event)
    {
        console.log("Item->touchEnd");
        _callEvent("ItemMoveEnd",this,this.node.convertToWorldSpaceAR(cc.v3(0,0,0)));
    },
});
