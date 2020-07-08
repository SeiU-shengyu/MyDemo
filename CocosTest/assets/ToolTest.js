// Learn cc.Class:
//  - https://docs.cocos.com/creator/manual/en/scripting/class.html
// Learn Attribute:
//  - https://docs.cocos.com/creator/manual/en/scripting/reference/attributes.html
// Learn life-cycle callbacks:
//  - https://docs.cocos.com/creator/manual/en/scripting/life-cycle-callbacks.html

var tools = require('Tools');

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
        this.stack = new tools.stack();
        this.queue = new tools.queue();
        this.list = new tools.list();

        this.stack.push(1);
        this.stack.push(2);
        this.stack.push(3);

        this.queue.push(1);
        this.queue.push(2);
        this.queue.push(3);

        this.list.push(1);
        this.list.push(2);
        this.list.push(3);

        console.log(this.stack.pop() + ":" + this.stack.size());
        console.log(this.stack.pop() + ":" + this.stack.size());
        console.log(this.stack.pop() + ":" + this.stack.size());
        console.log(this.queue.pop() + ":" + this.queue.size());
        console.log(this.queue.pop() + ":" + this.queue.size());
        console.log(this.queue.pop() + ":" + this.queue.size());
        console.log(this.list.getData(1) + ":" + this.list.size());
    },

    start () {

    },

    // update (dt) {},
});
