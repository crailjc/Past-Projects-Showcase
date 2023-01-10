// Call the aws site to get the history a specific dates
function getHistory() {
        var URL="https://crailjc.aws.csi.miamioh.edu/final.php?";
        var currDate = document.getElementById("date").value;
        var sortType = $( "#sort option:selected" ).text();
        var result = "";
        
        // makes sure the date is valid
        if (currDate.length != 10) {
            alert("invalid date please format as year-month-day");   
        }
        
        // check the sort type 
        if (sortType == "Sort by Zipcode") {
          sortType = 1;   
        } else {
          sortType = 2;
        }            
        
        a=$.ajax({
            url: URL,
            method: "GET",
            data: { method: "getTemp", location: "45056", date: currDate , sort: sortType}
            }).done(function(data) {  
                $("#historyText").text("");
                for (i = 0; i < data.result.length; i++) {
                   // create result string
                   result = "Location: " + data.result[i].location + " Date: " + data.result[i].date + "  DateRequested: " + data.result[i].DateRequested + "  Low: " +  data.result[i].low + "  High: " + data.result[i].high + "  Forecast: " + data.result[i].forecast; 
                   // add result to the paragraph tag and add a break line to the end of it 
                   $("#historyText").append(result + "<br> <br>");
                }
            }).fail(function(error) {
                alert("Invalid date");
                console.log("error",error.statusText);
            });
}