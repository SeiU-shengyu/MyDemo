// Learn cc.Class:
//  - https://docs.cocos.com/creator/manual/en/scripting/class.html
// Learn Attribute:
//  - https://docs.cocos.com/creator/manual/en/scripting/reference/attributes.html
// Learn life-cycle callbacks:
//  - https://docs.cocos.com/creator/manual/en/scripting/life-cycle-callbacks.html
const LoadResources = require("LoadResources");

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
        },
        scoreShow : 
        {
            default : null,
            type : cc.Label
        }
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.rodeGrids = this.node.children;
        this.curGridIndex = 0;
        this.walkLength = 0
        this.diceAnimation.on('finished',this.onDiceAnimationFinished,this)
        cc.systemEvent.on(cc.SystemEvent.EventType.KEY_DOWN,function(event){
            if(this.walkLength > 0 || this.score != this.lastScore)
                return;
            this.walkLength = Math.floor(Math.random() * 6 + 1);
            this.diceAnimation.play('Dice');
        }.bind(this),this);

        this.gridItems = [];
        for(let i = 0; i < this.rodeGrids.length;i++)
        {
            this.gridItems[i] = new Object();
            let node = new cc.Node("gridItem");
            node.setParent(this.rodeGrids[i]);
            this.gridItems[i]["node"] = node;
            this.gridItems[i]["sprite"] = node.addComponent(cc.Sprite);
            let index = Math.floor(Math.random() * LoadResources.itemIcons.length);
            this.gridItems[i].sprite.spriteFrame = LoadResources.itemIcons[index];
            this.gridItems[i]["score"] = index * 100;
            this.gridItems[i].node.width = 80;
            this.gridItems[i].node.height = 80;
        }
        this.score = 0;
        this.lastScore = 0;
        this.everyAddScore = 0;
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
            this.score += this.gridItems[this.curGridIndex].score;
            let index = Math.floor(Math.random() * LoadResources.itemIcons.length);
            this.gridItems[this.curGridIndex].sprite.spriteFrame = LoadResources.itemIcons[index];
            this.gridItems[this.curGridIndex].score = index * 100;
            this.gridItems[this.curGridIndex].node.width = 80;
            this.gridItems[this.curGridIndex].node.height = 80;
            if(this.walkLength == 0)
            {
                this.everyAddScore = (this.score - this.lastScore) / 10
                this.schedule(this.refreshScoreShow,0.05,9);
            }
            //console.log("walkLenght->" + this.walkLength + ":" + this.curGridIndex + "->" + this.rodeGrids[this.curGridIndex].name);
        },0.2,walkLength - 1);
    },
    refreshScoreShow()
    {
        this.lastScore += this.everyAddScore;
        if(this.lastScore > this.score)
            this.lastScore = this.score;
        this.scoreShow.string = this.lastScore;
    }
});
