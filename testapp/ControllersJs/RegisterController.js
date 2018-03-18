var myApp = angular.module("mainApp", []);

myApp.controller("registerController", ($scope, $http) => {

    $scope.isError = false;

    $scope.registrationSuccess = false;
    $scope.action = "Зарегистрироваться";

    $scope.types = [
        { value: "0", text: "* Примечание" },
        { value: "1", text: "Участник" },
        { value: "2", text: "Веду МК" },
        { value: "3", text: "Участник ярмарки" },
        { value: "4", text: "Веду лекцию/ семинар / круглый стол" }
    ];

    $scope.registerData = {
        email: "",
        password: "",
        surname: "",
        name: "",
        nik: "",
        city: "",
        comment: "",
        type: "0"
    };

    $scope.registerDisabled = () => {
        return $scope.registerData.email == "" ||
            $scope.registerData.password == "" ||
            $scope.registerData.surname == "" ||
            $scope.registerData.name == "" ||
            $scope.registerData.city == "" ||
            $scope.registerData.type === "0" ||
            $scope.inProcess;
    };

    $scope.register = () => {
        $scope.inProcess = true;
        $scope.action = "Идёт процесс..";
        $scope.isError = false;
        var isEmailCorrect = $scope.validateEmail($scope.registerData.email);
        if (!isEmailCorrect) {
            $scope.isError = true;
            $scope.errorMsg = "Введен некорректный e-mail!";
            $scope.inProcess = false;
            $scope.action = "Зарегистрироваться";
            return;
        }
        var dto = $scope.registerData;

        $http({
            method: 'POST',
            url: "/Main/AddUser",
            data: dto
        }).then((r) => {
            $scope.inProcess = false;
            $scope.action = "Зарегистрироваться";
            if (!r.data.success) {
                $scope.isError = true;
                $scope.errorMsg = r.data.errorMsg;
                return;
            }
            $scope.registrationSuccess = true;
        });
    };

    $scope.validateEmail = (email) => {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

});