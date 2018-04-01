var myApp = angular.module("mainApp", []);

myApp.controller("mainPageController", ($scope, $http) => {
    $scope.isError = false;

    $scope.action = "Войти";

    $scope.loginData = {
        email: "",
        password: ""
    };

    $scope.login = () => {
        $scope.inProcess = true;
        $scope.action = "Идёт процесс..";
        $scope.isError = false;

        var isEmailCorrect = $scope.validateEmail($scope.loginData.email);
        if (!isEmailCorrect) {
            $scope.isError = true;
            $scope.errorMsg = "Введен некорректный e-mail!";
            $scope.inProcess = false;
            $scope.action = "Войти";
            return;
        }
        var dto = $scope.loginData;

        $http({
            method: 'POST',
            url: "/Main/Login",
            data: dto
        }).then((r) => {
            $scope.inProcess = false;
            $scope.action = "Войти";
            if (!r.data.success) {
                $scope.isError = true;
                $scope.errorMsg = r.data.errorMsg;
                return;
            }
            else {
                window.location.replace("http://" + window.location.host + "/Main/UserPage");
            }
        });
    };

    $scope.validateEmail = (email) => {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }
});