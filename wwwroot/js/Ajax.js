var Controller="/"+"AjaxDataUpdate";

const EMode = Object.freeze({"replace":1, "after":2, "prepend":3, "GoalUpdate":4})

function getAjaxRequest(url, idHtml,data,mode,isScroll) 
{
    AjaxLocked=true;
    
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        async: true,
        success: function (callback) 
        {
            if (mode===EMode.replace)
            {
                jQuery("#" + idHtml).html(callback);
            }
            else if(mode===EMode.after)
            {
                jQuery("#" + idHtml).append(callback);
            }
            else if(mode===EMode.prepend) 
            {
                jQuery("#" + idHtml).prepend(callback);
            }
            else if(mode===EMode.GoalUpdate)
            {
                jQuery("#" + idHtml).html(callback);
                updateGoalLine();
            }
            
           if (isScroll)
           {
               DoubleScrollProtection=false;
           }
            
        },
        error: function () 
        {
            alert("Error ger response from server");
        }

    })
}
function changeGoalStatus(goalId,status,ajaxUpdate)
{
    var data = {"status": status,"goalId":goalId};
    

    getAjaxRequest(Controller+'/ChangeGoalStatus',ajaxUpdate,data,EMode.replace,false);
}


function doImportant(status,goalId,ajaxUpdate) 
{
    var data = {"status": status,"goalId":goalId};

    getAjaxRequest(Controller+'/DoImportant',ajaxUpdate,data,EMode.GoalUpdate,false);
    
}

function addGoal(isGoal,isDonate,title,body,donateValue,address) 
{
    var data = {"isGoal": isGoal,"isDonate":isDonate,"title":title,"body":body,"donateValue":donateValue,"address":address};
    
    getAjaxRequest(Controller+'/GoalAdd','allGoals',data,EMode.prepend,false);
    
}

function GetScroll(scrollNumber,userAddress) 
{
    
    var data = {"scrollNumber": scrollNumber,"address":userAddress,"status":StatusPage};

    if (PageName==="Profile")
    {
        getAjaxRequest(Controller+'/UpdateGoals','allGoals',data,EMode.after,true) ;
    }
    else if(PageName==="Feed")
    {
        getAjaxRequest(Controller+'/UpdateFeedGoals','allGoals',data,EMode.after,true) ;
    }
    
    

    

}

function GetSubscribers()
{

    var data = {"address":AddressPage};

    getAjaxRequest(Controller+'/GetSubscribers','Update-User-dialog',data,EMode.replace,false);

}

function GetSubscriptions()
{
    var data = {"address":AddressPage};

    getAjaxRequest(Controller+'/GetSubscriptions','Update-User-dialog',data,EMode.replace,false);

}

function updateGoalLine() 
{
    var data = {"address":AddressPage};

    getAjaxRequest(Controller+'/GoalLineUpdate','goalsLine',data,EMode.replace,false);
}

function subscribeToUser()
{
    var data = {"subscription":AddressPage,"status":true};

    getAjaxRequest(Controller+'/SetSubscriptionsStatus','User-All-Data',data,EMode.replace,false);
}

function unfollowUser() 
{
    var data = {"subscription":AddressPage,"status":false};

    getAjaxRequest(Controller+'/SetSubscriptionsStatus','User-All-Data',data,EMode.replace,false);

}