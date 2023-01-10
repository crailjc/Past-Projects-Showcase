document.addEventListener("DOMContentLoaded", function(){
var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var content = this.nextElementSibling;
    if (content.style.maxHeight){
      content.style.maxHeight = null;
    } else {
      content.style.maxHeight = content.scrollHeight + "px";
    } 
  });
}

});



// JS to close all divs if they are open 
// collapse all open content content adapted from 
function closeAll() {
  const cols = document.getElementsByClassName('collapsible');
  // for each loop to get all of the current elements 
  // of the collapsible class
  for (const col of cols) {
    if (col.classList.contains('active')) {
      col.classList.remove('active');
      toggle(col.nextElementSibling);
    }
  }
}
// toggle the content if its active 
// other wise the divs could no be closed again
function toggle(content) {
  if (content.style.maxHeight) {
    content.style.maxHeight = null;
  } else { 
    content.style.maxHeight = content.scrollHeight + 'px';
  }
}
