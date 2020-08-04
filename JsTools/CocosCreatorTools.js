//自定义事件中心(_eventsN分别包含不同参数个数的回调函数及对应的事件)
window._eventCenter = {
    _events1 : {},
    _events2 : {},
    _events3 : {},
    _events4 : {},
    _events5 : {},
    addEvent : function(eventType,call,argumentsLength)
    {
        if(!call)
        {
            console.error("no call");
            return;
        }
        switch(argumentsLength)
        {
            case 0:
                if(!this._events1.hasOwnProperty(eventType))
                    this._events1[eventType] = call;
                else
                    console.error("repeat event");
                break;
            case 1:
                if(!this._events2.hasOwnProperty(eventType))
                    this._events2[eventType] = call;
                else
                    console.error("repeat event");
                break;
            case 2:
                if(!this._events3.hasOwnProperty(eventType))
                    this._events3[eventType] = call;
                else
                    console.error("repeat event");
                break;
            case 3:
                if(!this._events4.hasOwnProperty(eventType))
                    this._events4[eventType] = call;
                else
                    console.error("repeat event");
                break;
            case 4:
                if(!this._events5.hasOwnProperty(eventType))
                    this._events5[eventType] = call;
                else
                    console.error("repeat event");
                break;
            default:
                console.error("not input argumentsLength");
                break;
        }
    },
    removeEvent : function(eventType,argumentsLength)
    {
        switch(argumentsLength)
        {
            case 0:
                if(this._events1.hasOwnProperty(eventType))
                    delete this._events1[eventType];
                else
                    console.error("no event");
                break;
            case 1:
                if(this._events2.hasOwnProperty(eventType))
                    delete this._events2[eventType];
                else
                    console.error("no event");
                break;
            case 2:
                if(this._events3.hasOwnProperty(eventType))
                    delete this._events3[eventType];
                else
                    console.error("no event");
                break;
            case 3:
                if(this._events4.hasOwnProperty(eventType))
                    delete this._events4[eventType];
                else
                    console.error("no event");
                break;
            case 4:
                if(this._events5.hasOwnProperty(eventType))
                    delete this._events5[eventType];
                else
                    console.error("no event");
                break;
            default:
                console.error("not input argumentsLength");
                break;
        }
    },
    callEvent : function(eventType)
    {
        switch(arguments.length)
        {
            case 1:
                if(this._events1.hasOwnProperty(eventType))
                    this._events1[eventType]();
                else
                    console.error("no event");
                break;
            case 2:
                if(this._events2.hasOwnProperty(eventType))
                    this._events2[eventType](arguments[1]);
                else
                    console.error("no event");
                break;
            case 3:
                if(this._events3.hasOwnProperty(eventType))
                    this._events3[eventType](arguments[1],arguments[2]);
                else
                    console.error("no event");
                break;
            case 4:
                if(this._events4.hasOwnProperty(eventType))
                    this._events4[eventType](arguments[1],arguments[2],arguments[3]);
                else
                    console.error("no event");
                break;
            case 5:
                if(this._events5.hasOwnProperty(eventType))
                    this._events5[eventType](arguments[1],arguments[2],arguments[3],arguments[4]);
                else
                    console.error("no event");
                break;
            default:
                console.error("not input eventType");
                break;
        }
    }
}

//加载资源
window._loadResources = {
    allAssets : {},
    remoteAssets : {},
    loadAllRes : function(path,progressCall)
    {
        cc.loader.loadResDir(path,function(completedCount,totalCounts){
            if(progressCall)
            progressCall(completedCount / totalCounts);
        },function(error,resources){
            console.log(resources.length);
            for(let i = 0;i < resources.length;i++)
            {
            console.log(resources[i].name)
            this.allAssets[resources[i].name] = resources[i];
            }
        }.bind(this));
    },
    loadRemoteAssets : function(url,call)
    {
        cc.loader.load(url, function (err, texture) {
            this.remoteAssets[url] = new cc.SpriteFrame(texture,cc.Rect(0,0,texture.width,texture.height));
            if(call)
                call(this.remoteAssets[url]);
        }.bind(this));
    },
    setRemoteSprite(sprite,name)
    {
        if(this.remoteAssets.hasOwnProperty(name))
            sprite.spriteFrame = this.remoteAssets[name];
        else
            this.loadRemoteAssets(name,function(spriteFrame){sprite.spriteFrame = spriteFrame;});
    }
}

//为目标子集下的按钮添加事件(不能有相同的名字)
window._addChildButtonEvent = function(target,targetNode)
{
    for(let i = 0; i < targetNode.children.length; i++)
    {
        let button = targetNode.children[i].getComponent(cc.Button);
        if(button)
        {
            let clickEventHandler = new cc.Component.EventHandler();
            clickEventHandler.target = target.node;
            clickEventHandler.component = cc.js.getClassName(target);
            console.log(cc.js.getClassName(target));
            clickEventHandler.handler = "on" + button.node.name; 
            button.clickEvents.push(clickEventHandler);
            target[button.node.name] = button;
        }
        else
        {
            if(targetNode.children[i].childrenCount != 0)
            {
                _addChildButtonEvent(target,targetNode.children[i]);
            }
        }
    }
}

//获取想要控制显示相关的节点和Lable、Sprite、Animation(以名字是否包含Show为基准,且不能有相同的名字)
window._getCtrlShowElement = function(target,targetNode)
{
    for(let i = 0;i < targetNode.children.length;i++)
    {
        if(targetNode.children[i].name.includes('Show'))
        {
            let label = targetNode.children[i].getComponent(cc.Label);
            let sprite = targetNode.children[i].getComponent(cc.Sprite);
            let animation = targetNode.children[i].getComponent(cc.Animation);
            if(label)
                target[label.node.name] = label;
            else if(sprite)
                target[sprite.node.name] = sprite;
            else if(animation)
                target[animation.node.name] = animation;
            else
                target[targetNode.children[i].name] = targetNode.children[i];
        }
        if(targetNode.children[i].childrenCount != 0)
        {
            _getCtrlShowElement(target,targetNode.children[i]);
        }
    }
}

//网络链接
window._xmlHttpRequest = function(url,data,code,codeNum,overTime)
{
    let xhr = new XMLHttpRequest();
    let p = new Promise(function(resolve,reject){
        xhr.open('POST',url,true);
        xhr.setRequestHeader("Content-type","application/json;charset=UTF-8");
        xhr.setRequestHeader("App-Channel",_urlData.channel);
        xhr.setRequestHeader("Authorization",_urlData.token);
        //xhr.setRequestHeader("Origin","http://localhost:7456");
        if(overTime)
            xhr.timeout = overTime;
        else
            xhr.timeout = 15000;
        xhr.onload = function(){
            let result = JSON.parse(xhr.responseText);
            if(result[code] == codeNum)
            {
                resolve(result);
            }
            else
            {
                reject(result);
            }
        }
        xhr.onerror = function(){
            reject('链接错误');
        }
        xhr.ontimeout = function(){
            reject('链接超时');
        }
        xhr.send(JSON.stringify(data));
    });
    return p;
}

//优化过的scrollView(限制content里面元素的描绘个数,有待改进,现在是每帧判定一次,可以改为只有在滑动的时候才判定)
window._scrollView = function(content,contentList,viewLength,singleHeight,space){
    let upDistance = content.position.y - viewLength / 2;
    let viewCounts = Math.ceil(viewLength / singleHeight);
    let upCounts = Math.floor(upDistance / singleHeight);

    for(let i = 0; i < contentList.length; i++)
    {
        if(i != 0)
            contentList[i].node.position = contentList[i].node.position.add(new cc.v3(0,-singleHeight,0));
        else
            contentList[i].node.position = new cc.v3(0,-contentList[i].node.height / 2 - space,0);

        let hideCounts = upCounts - 2 > 0 ? upCounts - 2 : 0;
        if(i < hideCounts)
            contentList[i].node.active = false;
        else if(i < hideCounts + 2 + viewCounts + 2)
            contentList[i].node.active = true;
        else
            contentList[i].node.active = false;
    }
}

//声音播放
window._mianVolume = 1;     //声音主音量,控制所有声音的大小
window._audioSources;       //声音播放池
//初始化声音池
window._initAudioSource = function(parent,counts)
{
    _audioSources = new _objectPool(null,parent,counts,'CustomerAudioSource')
}
//播放声音(audioClip参数必须设置,后面两个可以不设置且默认为1和false)
window._playSound = function(audioClip,volume,isLoop)
{
    let audioSource = _audioSources.get();
    audioSource.play(audioClip,volume,isLoop);
}

//获取1到100的整数
window._getRate = function(){
    return Math.floor(Math.random() * 100 + 1);
}
//获取最小和最大之间的数(不包括最大值)
window._getRandom = function(min,max){
    return Math.floor(Math.random() * (max - min) + min);
}
//获取几个数之间的一个数
window._getRandomValue = function(){
    return arguments[_getRandom(0,arguments.length)];
}

//对象池
//target:目标模板,根据这个创建节点,如果没有则创建默认节点
//parent:所有对象的父集
//counts:初始化的个数
//targetClass:目标类,与目标模板之间必须有一个.如果设置了,则对象池中存储的为该类的实例否则为节点
window._objectPool = function(target,parent,counts,targetClass)
{
    let busyPool = [],idlePool = [];
    for(let i = 0;i < counts;i++)
    {
        let targetObj;
        if(target)
            targetObj = cc.instantiate(target);
        else
            targetObj = new cc.Node();
        targetObj.parent = parent;
        targetObj.active = false;
        if(targetClass)
        {
            if(target)
                targetObj = targetObj.getComponent(targetClass);
            else
                targetObj = targetObj.addComponent(targetClass);
        }
        idlePool.push(targetObj);
    }
    this.get = function(){
        let targetObj;
        if(idlePool.length == 0)
        {
            if(target)
                targetObj = cc.instantiate(target);
            else
                targetObj = new cc.Node();
            targetObj.parent = parent;
            if(targetClass)
            {
                if(target)
                    targetObj = targetObj.getComponent(targetClass);
                else
                    targetObj = targetObj.addComponent(targetClass);
            }
        }
        else
        {
            targetObj = idlePool.pop();
        }
        busyPool.push(targetObj);
        if(targetObj instanceof cc.Node)
            targetObj.active = true;
        else
            targetObj.node.active = true;
        return targetObj;
    }
    this.release = function(target){
        let index = busyPool.indexOf(target);
        if(target instanceof cc.Node)
            target.active = false;
        else
            target.node.active = false;
        idlePool.push(target);
        busyPool.splice(index,1);
    }
}