(function(){
	var app = angular.module("app", ["ngMaterial", "ngAnimate", "ngAria"]);

	app.config(function() {

	});

	app.controller("MasterController", function($scope, $http) {
		this.issues = [];
		this.patterns = {};
		var that = this;

		this.currentPath = window.location.pathname;

		if(window.location.pathname.indexOf("/news/") > -1)
			this.sideAppMenuSelectedTabIndex = 0;
		else if(window.location.pathname.indexOf("/anti-pattern/") > -1)
			this.sideAppMenuSelectedTabIndex = 2;
		else
			this.sideAppMenuSelectedTabIndex = 1;
			
		/*if(window.location.pathname.indexOf("/news/") == -1)
			$http.get("https://api.github.com/repos/designpatternninja/designpatternninja.github.io/issues?labels=singleton&filter=all")
				.success(function(issues) {
					that.issues = issues;
				});*/
	});
})();