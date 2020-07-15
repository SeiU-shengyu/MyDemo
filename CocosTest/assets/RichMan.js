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
        curGridSprite : 
        {
            default : null,
            type : cc.Node
        },
        walkLengthShow :
        {
            default : null,
            type : cc.Label
        },
        diceAnimation : 
        {
            default : null,
            type : cc.Animation
        }
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.rodeGrids = this.node.children;
        this.curGridIndex = 0;
        this.walkLength = 0
        this.diceAnimation.on('finished',this.onDiceAnimationFinished,this)
        cc.systemEvent.on(cc.SystemEvent.EventType.KEY_DOWN,function(event){
            if(this.walkLength > 0)
                return;
            this.walkLength = Math.floor(Math.random() * 6 + 1);
            this.diceAnimation.play('Dice');
        }.bind(this),this);

        
        cc.loader.loadResDir("texture",cc.SpriteFrame,function(error,spriteFrames)
        {
            for(let i = 0;i < spriteFrames.length;i++)
            {
                this.itemIcons[i] = spriteFrames[i];
            }
            this.items = new Array(spriteFrames.length);
            console.log("loadImage:" + this.itemIcons.length);
        }.bind(this))
    },

    start () {

    },

    onDiceAnimationFinished()
    {
        this.walkLengthShow.string = this.walkLength;
        this.walk(this.walkLength);
    },

    // update (dt) {},
    walk(walkLength)
    {
        this.schedule(function()
        {
            this.curGridIndex = ++this.curGridIndex == this.rodeGrids.length ? 0 : this.curGridIndex;
            this.curGridSprite.position = this.rodeGrids[this.curGridIndex].position;
            this.walkLength--;
            console.log(this.curGridIndex + "->" + this.rodeGrids[this.curGridIndex].name);
        },0.2,walkLength - 1);
    }
});
