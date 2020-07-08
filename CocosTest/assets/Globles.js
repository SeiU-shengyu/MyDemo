_events1 = {}
_events2 = {}
_events3 = {}
_events4 = {}
_events5 = {}
_addEvent = function(eventType,call,argumentsLength)
{
    if(!call)
    {
        console.error("no call");
        return;
    }
    switch(argumentsLength)
    {
        case 0:
            if(!_events1.hasOwnProperty(eventType))
                _events1[eventType] = call;
            else
                console.error("repeat event");
            break;
        case 1:
            if(!_events2.hasOwnProperty(eventType))
                _events2[eventType] = call;
            else
                console.error("repeat event");
            break;
        case 2:
            if(!_events3.hasOwnProperty(eventType))
                _events3[eventType] = call;
                else
                    console.error("repeat event");
                break;
        case 3:
            if(!_events4.hasOwnProperty(eventType))
                _events4[eventType] = call;
            else
                console.error("repeat event");
            break;
        case 4:
            if(!_events5.hasOwnProperty(eventType))
                _events5[eventType] = call;
            else
                console.error("repeat event");
            break;
    }
}
_removeEvent = function(eventType,argumentsLength)
{
    switch(argumentsLength)
    {
        case 0:
            if(_events1.hasOwnProperty(eventType))
                delete _events1[eventType];
            else
                console.log("no event");
            break;
        case 1:
            if(_events2.hasOwnProperty(eventType))
                delete _events2[eventType];
            else
                console.log("no event");
            break;
        case 2:
            if(_events3.hasOwnProperty(eventType))
                delete _events3[eventType];
            else
                console.log("no event");
            break;
        case 3:
            if(_events4.hasOwnProperty(eventType))
                delete _events4[eventType];
            else
                console.log("no event");
            break;
        case 4:
            if(_events5.hasOwnProperty(eventType))
                delete _events5[eventType];
            else
                console.log("no event");
            break;
    }
}
_callEvent = function(eventType)
{
    switch(arguments.length)
    {
        case 1:
            if(_events1.hasOwnProperty(eventType))
                _events1[eventType]();
            else
                console.log("no event");
            break;
        case 2:
            if(_events2.hasOwnProperty(eventType))
                _events2[eventType](arguments[1]);
            else
                console.log("no event");
            break;
        case 3:
            if(_events3.hasOwnProperty(eventType))
                _events3[eventType](arguments[1],arguments[2]);
            else
                console.log("no event");
            break;
        case 4:
            if(_events4.hasOwnProperty(eventType))
                _events4[eventType](arguments[1],arguments[2],arguments[3]);
            else
                console.log("no event");
            break;
        case 5:
            if(_events5.hasOwnProperty(eventType))
                _events5[eventType](arguments[1],arguments[2],arguments[3],arguments[4]);
            else
                console.log("no event");
            break;
    }
}