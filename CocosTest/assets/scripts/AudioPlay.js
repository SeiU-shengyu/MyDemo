// Learn cc.Class:
//  - https://docs.cocos.com/creator/manual/en/scripting/class.html
// Learn Attribute:
//  - https://docs.cocos.com/creator/manual/en/scripting/reference/attributes.html
// Learn life-cycle callbacks:
//  - https://docs.cocos.com/creator/manual/en/scripting/life-cycle-callbacks.html

var tools = require('Tools')

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
        this.audioSourcePool = new cc.NodePool(cc.AudioSource);
        for(let i = 0; i < 20; i++)
        {
            let node = new cc.Node("aduio" + i);
            node.setParent(this.node);
            let customerAudio = node.addComponent('CustomerAudioSource');
            customerAudio.node.on('playend',this.recycleAudio.bind(this),customerAudio);
            this.audioSourcePool.put(node);
        }

        this.audioClips1 = [];
        this.audioClips2 = [];
        this.audioClips3 = [];
        this.playQueue = new tools.queue();
        this.playCounts = 0;
        this.lastPlayTime = 0;

        cc.loader.loadResDir('track',cc.AudioClip,function(error,resource)
        {
            this.audioClips1[0] = resource[1];
            this.audioClips1[1] = resource[0];
            this.audioClips1[2] = resource[2];

            this.audioClips2[0] = resource[3];
            this.audioClips2[1] = resource[4];
            this.audioClips2[2] = resource[5];
            this.audioClips2[3] = resource[6];
            this.audioClips2[4] = resource[7];
            this.audioClips2[5] = resource[8];
            this.audioClips2[6] = resource[9];
            this.audioClips2[7] = resource[10];

            // this.schedule(function(){
            //     let audioSource = this.audioSourcePool.get().getComponent('CustomerAudioSource');
            //     audioSource.node.setParent(this.node);
            //     audioSource.clip = this.audioClips1[0];
            //     audioSource.play();
            // }.bind(this),0.3,cc.macro.REPEAT_FOREVER)

            // this.schedule(function(){
            //     this.playBgm()
            // }.bind(this),0.2,cc.macro.REPEAT_FOREVER);
        }.bind(this))

        cc.loader.loadResDir('main',cc.AudioClip,function(error,resource)
        {
            for(let i = 0; i < resource.length; i++)
                this.audioClips3[i] = resource[i];
            cc.systemEvent.on(cc.SystemEvent.EventType.KEY_DOWN,this.onKeyDown,this)
            cc.systemEvent.on(cc.SystemEvent.EventType.KEY_UP,this.onKeyUp,this)
        }.bind(this))

        this.noPlaying = true;
    },

    start () {
    },

    update (dt) {
    },

    recycleAudio(audioSource)
    {
        this.audioSourcePool.put(audioSource.node);
    },

    playBgm()
    {
        let audioSource = this.audioSourcePool.get().getComponent('CustomerAudioSource');
        audioSource.node.setParent(this.node);
        let playIndex = 0;
        if(this.playCounts < 4)
        {
            playIndex = Math.floor(Math.random() * 8)
            audioSource.clip = this.audioClips2[playIndex];
            this.playCounts++;
        }
        else
        {
            audioSource.clip = this.audioClips1[2];
            this.playCounts = 0;
        }
        audioSource.play();
    },

    onKeyDown(event)
    {
        let index = Math.abs(event.keyCode - (this.audioClips3.length - 1));
        console.log(event.keyCode + ":" + index + ":" + this.audioClips3.length);
        if(index >= this.audioClips3.length)
            index -= parseInt(index / (this.audioClips3.length -1)) * (this.audioClips3.length -1)

        if(this.noPlaying)
        {
            this.noPlaying =false;
            this.playClickAudio(index);
            this.scheduleOnce(function(){
                this.playNextAudio();
            }.bind(this),0.15)
            console.log("Play");
        }
        else
        {
            this.playQueue.push(index);
            console.log("push");
        }
    },

    onKeyUp(event)
    {
        this.playQueue.clear();
    },

    playNextAudio()
    {
        if(this.playQueue.size() == 0)
        {
            this.noPlaying = true;
            console.log("playend");
        }
        else
        {
            let index = this.playQueue.pop();
            this.playClickAudio(index);
            this.scheduleOnce(function(){
                this.playNextAudio();
            }.bind(this),0.15)
            console.log("PlayNext");
        }
    },

    playClickAudio(index)
    {
        let audioSource = this.audioSourcePool.get().getComponent('CustomerAudioSource');
        audioSource.node.setParent(this.node);
        audioSource.clip = this.audioClips3[index];
        audioSource.play();
    }
});
