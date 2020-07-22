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
        // this.loadDirRes("texture",cc.SpriteFrame).then(function(resource){
        //   console.log(resource.length);  
        // },console.error);
        let promise1 = this.getUrlMsg("http://test-uc-api.0dgame.com/passport/login","errcode",200,JSON.stringify({username:"15201975683",password:"2huoshax",type:"account"}));
        let promise2 = this.getUrlMsg("http://test-uc-api.0dgame.com/passport/login","errcode",200,JSON.stringify({username:"13100000001",password:"123456",type:"account"}));
        let promise3 = this.getUrlMsg("http://test-uc-api.0dgame.com/passport/login","errcode",200,JSON.stringify({username:"13100000002",password:"123456",type:"account"}));
        Promise.all([promise1,promise2,promise3]).then(function(values){
            for(var value in values)
                console.log(JSON.stringify(values[value]));
        })
    },

    start () {

    },

    // update (dt) {},
    loadDirRes(path,type)
    {
        let p = new Promise(function(resolve,reject){
            cc.loader.loadResDir(path,type,function(error,resource){
                if(error)
                    reject(error);
                else
                    resolve(resource);
            })
        });
        return p;
    },
    loadRes(path,type)
    {
        let p = new Promise(function(resolve,reject){
            cc.loader.loadRes(path,type,function(error,resource){
                if(error)
                    reject(error);
                else
                    resolve(resource);
            })
        });
        return p;
    },
    getUrlMsg(url,code,number,data){
        var xhr = new XMLHttpRequest();
        xhr.timeout = 15000;
        let p = new Promise(function(resolve,reject){
            xhr.open('POST',url,true);
            xhr.setRequestHeader("Content-type","application/json;charset=UTF-8");
            xhr.setRequestHeader("App-Channel",100001);
            xhr.setRequestHeader("Origin","http://localhost:7456");
            xhr.onload = function(){
                if(this.status == 200)
                {
                    let result = JSON.parse(xhr.responseText);
                    if(result[code] == number)
                        resolve(result);
                    else
                        reject(result);
                }
            };
            xhr.onerror = function(){
                reject("链接错误");
            };
            xhr.ontimeout = function(){
                reject("链接超时");
            };
            xhr.send(data);
        });
        return p;
    }
});
