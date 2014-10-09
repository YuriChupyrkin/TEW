$(function () {
  
  (function getInfo() {
    $('#loadingContainer').removeClass("display-none");
    $('#statContainer').addClass("display-none");

    var usersCount = 0;
    var wordsCount = 0;

    $.getJSON("/api/TewInfo", function(result) {
      $('#loadingContainer').addClass("display-none");
      $('#statContainer').removeClass("display-none");
      
      usersCount = result.Users;
      wordsCount = result.Words;

      $('#userCount').text("users: " + usersCount);
      $('#wordsCount').text("words: " + wordsCount);
    });
  })();


});