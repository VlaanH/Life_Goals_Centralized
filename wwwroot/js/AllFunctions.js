function imageBinding(input,ImageID)
{

    var img = new Image();
    img.src = input.value;
    img.onload = function(){document.getElementById(ImageID).src = input.value};
    img.onerror = function(){document.getElementById(ImageID).src='/UserImages/standardUser.png'};

}
function DoImportant(id,status,ajaxUpdate)
{
    doImportant(status,id,ajaxUpdate);
   
}
function SetDescription()
{
    var description = document.getElementById('SettingsDescription').value.replace(/"/g, "“");
    Web3SetDescription(description);

}
function SetUserBackground()
{
    var imag = document.getElementById('SettingsBackground').value;

    Web3SetBackground(imag);
}
function SetImag()
{
    var imag = document.getElementById('SettingsImage').value;

    Web3SetImag(imag);
}

function AddGoal()
{
    var isGoal= document.getElementById('isGoal').checked;
    var isDonate = document.getElementById('isDonate').checked;
    var title = document.getElementById('CreateGoalTitle').value.replace(/"/g, "“");
    var body = document.getElementById('CreateGoalBody').value.replace(/"/g, "“");
    var donateValue = document.getElementById('CreateGoalDonateValue').value;
    var address = document.getElementById('CreateGoalAddress').value;

    if (isDonate)
    {
        if (address.length===42&& address[0]==='0' && address[1]==='x')
        {
            addGoal(isGoal,isDonate,title,body,donateValue,address);
        }
        else
        {
            alert("invalid wallet");
        }
        
    }
    else 
    {
        addGoal(isGoal,isDonate,title,body,donateValue,address);
    }

}


function ChangeGoalStatus(id,status,ajaxUpdate)
{
    changeGoalStatus(id,status,ajaxUpdate);
}
function SubscribeToUser()
{
    subscribeToUser();
}
function UnfollowUser()
{
    unfollowUser();
}