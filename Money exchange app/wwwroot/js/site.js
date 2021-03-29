function ToExchange(e)
{
    e.preventDefault();
    const form = document.forms.ExchangeForm;
    var fromAmount = form.From;
    var toAmount = form.To;
    var fromCurrency = form.FromCurrency.value;
    var toCurrency = form.ToCurrency.value;

    if (fromAmount.value == "" && toAmount.value == "")
    {
        alert("Enter any value!");
    }
    else if (fromAmount.value != "" && toAmount.value == "")
    {
        Procces(fromCurrency, toCurrency, fromAmount.value, toAmount);
    }
    else if (fromAmount.value == "" && toAmount.value != "")
    {
        Procces(toCurrency, fromCurrency, toAmount.value, fromAmount);
    } else if (fromCurrency == toCurrency )
    {
        toAmount.value = fromAmount.value;
    }
    else
    {
        Procces(fromCurrency, toCurrency, fromAmount.value, toAmount);
    }
}
function Procces(fromData, toData, amount,locationForSet)
{
    const data = new FormData();
    data.append("fromData", fromData);
    data.append("toData", toData);
    data.append("amount", amount);
    var xhr = new XMLHttpRequest();
    xhr.open("post", "Home/Index", true);
    xhr.onload = function (data)
    {
        if (xhr.status === 200)
        {
            console.log(xhr.responseText);
            locationForSet.value = Number(xhr.responseText) * Number(amount);
            const form = document.forms.ExchangeForm;
            document.getElementById('cost').innerHTML = "1 " + fromData + " = " + xhr.responseText + " " + toData;
            return;
        }
        if (xhr.status === 500)
        {
            document.getElementById('cost').innerHTML =JSON.parse(xhr.responseText).detail;            
        }
        locationForSet.value = "Error";
    }
    xhr.send(data);
}
function swap()
{
    const form = document.forms.ExchangeForm;
    var fromCurrency = form.FromCurrency;
    var toCurrency = form.ToCurrency;
    var temp = fromCurrency.value;
    fromCurrency.value = toCurrency.value;
    toCurrency.value = temp;
}
function SetFilter()
{
    var id = document.getElementById("Id");
    var FromCurrency = document.getElementById("FromCurrency");
    var FromAmount = document.getElementById("FromAmount");
    var ToCurrency = document.getElementById("ToCurrency");
    var ToAmount = document.getElementById("ToAmount");
    var Date = document.getElementById("Date");

    const data = {
        Id: id.value,
        FromCurrency: FromCurrency.value,
        FromAmount: FromAmount.value,
        ToCurrency: ToCurrency.value,
        ToAmount: ToAmount.value,
        Date: Date.value
    };
    var req=serialize(data);
    window.location="/Home/History/?" + req;
}
serialize = function (obj)
{
    var str = [];
    for (var p in obj)
        if (obj.hasOwnProperty(p))
        {
            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
        }
    return str.join("&");
}