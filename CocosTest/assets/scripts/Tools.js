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
}

module.exports = tools;
