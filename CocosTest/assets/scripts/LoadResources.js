var loadResources = 
{
    progress : 0,
    itemIcons : [],
    load : function(completedCall){
        cc.loader.loadResDir("texture",cc.SpriteFrame,function(completeCounts,totalCounts,item){
            progress = completeCounts / totalCounts * 100;
            console.log(progress);
        },function(error,spriteFrames)
        {
            for(let i = 0;i < spriteFrames.length;i++)
            {
                this.itemIcons[i] = spriteFrames[i];
            }
            if(completedCall)
                completedCall();
        }.bind(this))
    }
}

module.exports = loadResources;