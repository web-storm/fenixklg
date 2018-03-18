var myApp = angular.module("mainApp", []);

myApp.controller("mainPageController", ($scope) => {
    $scope.test = "Hello, world!";
});