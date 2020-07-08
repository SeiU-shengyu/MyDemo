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
        this.lastPosition = this.node.position;
    },

    start () {

    },

    // update (dt) {},

    onTouchStart(event)
    {
        console.log("Item->touchStart");
    },
    onTouchMove(event)
    {
        console.log("Item->touchMove");
        this.node.position = this.node.parent.convertToNodeSpaceAR(event.getLocation());
    },
    onTouchEnd(event)
    {
        console.log("Item->touchEnd");
        _callEvent("ItemMoveEnd",this.node,this.node.convertToWorldSpaceAR(cc.v3(0,0,0)),this.lastPosition);
        this.lastPosition = this.node.position;
    },
});