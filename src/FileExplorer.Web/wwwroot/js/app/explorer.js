var app = angular.module('explorerApp', []);

app.controller('explorerController', function ($scope, $http) {
    $scope.changeDir = function (path) {
        $http.get("/api/explorer?path=" + path)
            .then(function (response) {
                $scope.data = response.data;
            });       
    };
    $scope.changeDir("");
});