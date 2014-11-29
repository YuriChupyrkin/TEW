$(function () {
  var serverPath = "";

  (function getInfo() {
    $('#loadingContainer').removeClass("display-none");
    $('#statContainer').addClass("display-none");

    var usersCount = 0;
    var wordsCount = 0;

    //$.getJSON(serverPath + "/api/TewInfo", function (result) {
    $.getJSON(serverPath + "/TewCloud/api/TewInfo", function(result) {  
      $('#loadingContainer').addClass("display-none");
      $('#statContainer').removeClass("display-none");
      
      usersCount = result.Users;
      wordsCount = result.Words;

      $('#userCount').text("users: " + usersCount);
      $('#wordsCount').text("words: " + wordsCount);
    });
  })();

  $(document).on('click', '#downloadButton', function (event) {
      console.log("download");
      window.location = "#download";
  });
  

  $(document).on('click', '#indexHeader', function (event) {
    var currentLocation = window.location.href;
    var irregularVerbsPath = '/irregular_verbs.html';
    
    window.location = currentLocation + irregularVerbsPath;
  });
});