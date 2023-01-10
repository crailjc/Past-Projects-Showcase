var latitude;
var longitude;
var dailyDate;
var Zipcode

// Main function to handle getting all the information
// and then calling other functions to formate and place the data
function getWeather() {
        var URL="https://api.clearllc.com/api/v2/miamidb/_table/zipcode?";
        Zipcode = document.getElementById("zipcode").value;
        
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
                // alert(JSON.stringify(data.resource));   
                latitude = data.resource[0].latitude;
                longitude = data.resource[0].longitude; 
                callWeatherAPI();
                
            }).fail(function(error) {
                alert("Invalid zipcode");
                console.log("error",error.statusText);
            });
 }
 
// This is the function that calls the weather API and gets the
// forecast for the next seven days
function callWeatherAPI() {
        var URL = "https://api.openweathermap.org/data/2.5/onecall?";  
        var img;
        var unit = $( "#units option:selected" ).text();
        var unitSymb = unit.substring(unit.length-2, unit.length-1);        
        unit = unit.substring(0, unit.length-4);
        
        a=$.ajax({
            url: URL,
            method: "GET",
            data: { lat: latitude, lon: longitude, exclude: "hourly", appid: "224b11fe47b27255a6060d232a1d6ba4", units: unit}
            }).done(function(data) {
               for (i =1; i <= 7; i++) {
                   $("#weather" + i).html("High " +  data.daily[i-1].temp.max + "&#176; " + "Low " +  data.daily[i-1].temp.min + "&#176; " + data.daily[i].weather[0].description);
                   document.getElementById('img' + i).src = "http://openweathermap.org/img/wn/" + data.daily[i].weather[0].icon + "@2x.png";
                   setDate(data.daily[i-1].sunrise, data.daily[i-1].sunset, i);
                   setTemp(dailyDate, Zipcode, data.daily[i-1].temp.min, data.daily[i-1].temp.max, data.daily[i].weather[0].description, unitSymb);
               }
            }).fail(function(error) {
                alert("Invalid zipcode");
                console.log("error",error.statusText);
            });
}

// Method to send a request to the aws php file to add 
// the temps to the current temp list
 function setTemp(dailyDate, dailyLocation, low, high, dailyforecast, unit) {
    var URL = "https://crailjc.aws.csi.miamioh.edu/final.php?"; 
        
    a=$.ajax({
        url: URL,
        method: "GET",
        data: { method : "setTemp", date: dailyDate, location: dailyLocation, low: (low + " " + unit), high: (high + " " + unit), forecast: dailyforecast}
        }).done(function(data) {
           // If good and no errors nothing will need to be done

        }).fail(function(error) {
            alert("Invalid setTemp");
            console.log("error",error.statusText);
        }); 
}

// Method to convert the epoch date to values that are usable
function setDate(sunrise, sunset, i) {
    var d = new Date(0); // 0 sets the date to epoch
    var e = new Date(0); 
    var sunRtime, sunStime, humanDate;
    bufferMonth = "", bufferDay = "";
    d.setUTCSeconds(sunrise);
    humanDate = getHumanDate(d.getMonth(), d.getDay(), d.getDate());
    
    // make sure that the month and day are not single digits
    if ((d.getMonth()+1) < 10) {
        bufferMonth = "0";
    }   
    
    if (d.getDate() < 10) {
        bufferDay = "0";
    }
     
    dailyDate = (d.getFullYear() + "-" + bufferMonth+ (d.getMonth()+1) + "-"+ bufferDay+ d.getDate());
    sunRtime = d.getHours() +":" + d.getMinutes();
    e.setUTCSeconds(sunset);
    // Make sure the minutes are not single digit 
    if (d.getMinutes() < 10) {
        sunStime = e.getHours() +":0" + e.getMinutes();
    } else {
        sunStime = e.getHours() +":" + e.getMinutes();
    }
    // add values to the date and suntime sections
    $("#date" + i).html(humanDate);
    $("#sun" + i).html("Sunrise: " + sunRtime + " Sunset: " +sunStime);
}

// Takes the numerical values and changes them into
// thier text counter part
function getHumanDate(month, day, date) {
   var dayWord, monthWord
   if (day == 0) {
     dayWord = "Sunday";
   } else if (day == 1) { 
     dayWord = "Monday";
   } else if (day == 2) {
     dayWord = "Tuesday";
   } else if (day == 3) {
     dayWord = "Wednesday";
   } else if (day == 4) {
     dayWord = "Thursday"; 
   } else if (day == 5) {
     dayWord = "Friday";
   } else {
     dayWord = "Saturday";
   }
    
   if (month == 0) {
       monthWord = "January";
   } else if (month == 1) {
       monthWord = "Febuary";
   } else if (month == 2) {
       monthWord = "March";
   } else if (month == 3) {
       monthWord = "April";
   } else if (month == 4) {
       monthWord = "May";
   } else if (month == 5) {
       monthWord = "June";
   } else if (month == 6) {
       monthWord = "July";
   } else if (month == 7) {
       monthWord = "August";
   } else if (month == 8) {
       monthWord = "Semptember";
   } else if (month == 9) {
       monthWord = "October";
   } else if (month == 10) {
       monthWord = "Novermber";
   } else if (month == 10) {
       monthWord = "December";
   }
   return (dayWord + ", " + monthWord + " " +  date);
}








