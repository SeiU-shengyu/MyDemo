var tools = {
    stack :function(){
        let Node = function(data){
            this.data = data;
            this.next = null;
        }
    
        let length = 0,top;
    
        this.push = function(data){
            let node = new Node(data);
            if(top)
                node.next = top;
            top = node;
            length++;
        }
    
        this.pop = function(){
            if(top){
                let current = top;
                top = current.next;
                length--;
                return current.data;
            }
            else{
                console.error("null stack")
            }
        }
    
        this.size = function(){
            return length;
        }
    
        this.clear = function(){
            top = null;
            length = 0;
        }
    },

    queue : function(){
        let Node = function(data){
            this.data = data;
            this.next = null;
        }
    
        let length = 0,front,rear;
    
        this.push = function(data){
            let node = new Node(data);
            if(rear)
                rear.next = node;
            rear = node;
            if(!front)
            front = rear;
            length++
        }
    
        this.pop = function(){
            if(front){
                let current = front;
                front = current.next;
                length--;
                return current.data;
            }
            else{
                console.error("null queue");
            }
        }
    
        this.size = function(){
            return length;
        }
    
        this.clear = function(){
            front = null;
            rear = null;
            length = 0;
        }
    },

    list : function(){
        let datas = [];
        this.push = function(data){
            datas.push(data);
        }
        this.remove = function(data){
            for(let i = 0;i < datas.length;i++){
                if(datas[i] === data){
                    for(let j = i;j < datas.length - 1;j++){
                        datas[j] = datas[j + 1];
                    }
                    datas.length -= 1;
                }
            }
        }
        this.removeAtIndex = function(index){
            if(index >= datas.length)
                console.error("no index");
            else{
                for(let i = index;i < datas.length - 1;i++){
                    datas[i] = datas[i + 1];
                }
                datas.length -= 1;
            }
        }
        this.clear = function(){
            datas.length = 0;
        }
        this.size = function(){
            return datas.length;
        }
        this.getData = function(index){
            if(index >= datas.length)
                console.error("no index");
            else
                return datas[index];
        }
    },
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
            xhr.onload = function(e){
                if(this.status == 200)
                {
                    let result = JSON.parse(e.responseText);
                    if(result[code] == number)
                        resolve(result);
                    else
                        reject(result);
                }
            };
            xhr.onerror = function(e){
                reject(e);
            };
            xhr.ontimeout = function(e){
                reject(e);
            }
            xhr.send(data);
        });
        return p;
    },
}

module.exports = tools;
