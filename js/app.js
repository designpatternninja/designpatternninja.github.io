(function(){
	var app = angular.module("app", ["ngMaterial", "ngAnimate", "ngAria"]);

	app.config(function() {

	});

	app.controller("MasterController", function($scope, $http) {
		this.issues = [];
		this.patterns = {};
		var that = this;

		this.currentPath = window.location.pathname;

		this.sideAppMenuSelectedTabIndex = window.location.pathname.indexOf('/news/') > -1 ? 0 : 1;
		/*if(window.location.pathname.indexOf("/news/") == -1)
			$http.get("https://api.github.com/repos/designpatternninja/designpatternninja.github.io/issues?labels=singleton&filter=all")
				.success(function(issues) {
					that.issues = issues;
				});*/
	});
})();