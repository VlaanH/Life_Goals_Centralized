function getAjaxPage(url, idHtml,jsonData)
{

    jQuery.ajax({
                url: url,
                data: jsonData,
                type: "POST",
                success: function (callback) {
                    jQuery("#" + idHtml).html(callback);
                    
                },
                error: function () {
                    alert("Error ger response from server");
                }
    
            });
}
function AddHistory(transitionUrl,dataHistory) 
{
    var thisPage=window.location.pathname+window.location.search;
    
    if (thisPage!==transitionUrl)
        history.pushState(dataHistory, null, transitionUrl);
    else
    {
        history.replaceState(dataHistory, null, transitionUrl);
    }
}

function RefreshAjaxPage() 
{
    var thisPage=history.state.PageName;
    
    var pageAddress=history.state.address;

    PageAjaxTransition(thisPage,pageAddress,true);
    
}

function PageAjaxTransition(ajaxUrl,address,isAddressPage=false)
{

    var addressJson={"address":address};
    
    var dataHistory,transitionUrl;
    
    if (isAddressPage===false)
    {
        SetBackground("default");
        getAjaxPage(/AjaxPageTransition/+ajaxUrl, "MainFrame");

        transitionUrl="/home/"+ajaxUrl;

        dataHistory={'PageName': ajaxUrl};
        
    }
    else 
    {
        ScrollNumber=2;
        getAjaxPage(/AjaxPageTransition/+ajaxUrl, "MainFrame",addressJson);

        transitionUrl="/home/"+ajaxUrl+"?address="+address;

        dataHistory={'PageName': ajaxUrl,'address':address};
        
    }
    
    
    if (isAddressPage)
    {
        if (address!=null)
        {
            AddHistory(transitionUrl,dataHistory,isAddressPage)
        }
    }
    else 
    {
        AddHistory(transitionUrl,dataHistory,isAddressPage)
    }
   
    
    return false;
}

$(window).on('load', function()
{

    $('a').bind('click', function()
    {
        var namePage = $(this).attr("page");
        var isAddressTeg=$(this).attr("Address");
        
        if (namePage!=null)
        {
            
            if (isAddressTeg==="true")
            {
                if (userAddress!='')
                    PageAjaxTransition(namePage,userAddress,true);
                else
                    window.location.href = "/Identity/Account/Login";
                    
            }
            else
            {
                PageAjaxTransition(namePage);
            }
            
            return false;
        }
       
        
    });
   
});
window.onpopstate = function(event)
{

    if (event.state==null)
    {
        window.location=document.location;

    }
    else
    {
        SetBackground("default");
        PageAjaxTransition(event.state.PageName,event.state.address,true)
    }

};

window.onload = async () => {

    if (typeof (initPage) === "function") {
        initPage();
    }

};

function hidden(id,isHidden)
{
    try
    {
        if (isHidden)
        {
            document.getElementById(id).classList.add("hidden");
        }
        else
        {
            document.getElementById(id).classList.remove("hidden");
        }
    }
    catch (e)
    {

    }


}
