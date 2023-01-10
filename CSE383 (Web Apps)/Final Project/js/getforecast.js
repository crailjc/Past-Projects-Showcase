 
 
    function getForecast() {
        var URL="https://api.clearllc.com/api/v2/miamidb/_table/zipcode?";
        var Zipcode = document.getElementById("zipcode").value;
        
        if (Zipcode == " " || Zipcode == "") {
            alert("Please enter a zipcode"); 
            // Stop processing the rest of the function
            return;
        }
        
        
        a=$.ajax({
            url: URL,
            method: "GET",
            data: { api_key: "bed859b37ac6f1dd59387829a18db84c22ac99c09ee0f5fb99cb708364858818", ids: Zipcode}
            }).done(function(data) {
                alert(JSON.stringify(data.resource));   
                $("#city").html(data.resource[0].city);
                $("#state").html(data.resource[0].state);
                $("#long").html(data.resource[0].longitude);
                $("#lat").html(data.resource[0].latitude);  
                $("#time").html(data.resource[0].timezone + " UTC");
                $("#day").html(data.resource[0].daylightSavingsFlag);
                $("#geo").html(data.resource[0].geopoint)


            }).fail(function(error) {
                alert("Invalid zipcode");
                console.log("error",error.statusText);
            });
    }

