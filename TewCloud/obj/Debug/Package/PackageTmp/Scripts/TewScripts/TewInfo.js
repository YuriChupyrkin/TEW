$(function () {
	var serverPath = "";

	(function getInfo() {
		$('#loadingContainer').removeClass("display-none");
		$('#statContainer').addClass("display-none");

		var usersCount = 0;
		var wordsCount = 0;

		$.getJSON(serverPath + "/api/TewInfo", function (result) {
			$('#loadingContainer').addClass("display-none");
			$('#statContainer').removeClass("display-none");

			usersCount = result.Users;
			wordsCount = result.Words;

			$('#userCount').text("users: " + usersCount);
			$('#wordsCount').text("words: " + wordsCount);
		});
	})();

	$(document).on('click', '#downloadButton', function (event) {
		var win = window.open("https://www.dropbox.com/s/2yyci32ldmys3vf/TewLauncherAzure.rar", '_blank');
		win.focus();
	});

	$(document).on('click', '#irregularVerbsButton', function (event) {
		var currentLocation = window.location.href;
		var irregularVerbsPath = '/irregular_verbs.html';

		var ulr = currentLocation + irregularVerbsPath;

		var win = window.open(ulr, '_blank');
		win.focus();
	});
});