var myApp = angular.module("mainApp", []);

myApp.controller("userPageController", ($scope, $http) => {

    $scope.userInfo = {
        Name: "",
        Surname: "",
        Nik: "",
        IsActive: false,
        Balance: 0,
        IsBank: false,
        Code: "",
    }

    $scope.news = [{
        Title: "Новость",
        Date: "01-04-2018",
        Content: "Тестовая новость!"
    },
    {
        Title: "Новость1",
        Date: "01-04-2018",
        Content: "Тестовая новость1!"
    }];

    $scope.wishSended = false;
    $scope.greetingName = "";
    $scope.greeting = "Загружаем данные..";

    $scope.getUserInfo = () => {
        $http({ method: "POST", url: "GetUserInfo" })
            .then((r) => {
                if (r.data.success) {
                    $scope.userInfo = r.data.userInfo;
                    $scope.userInfo.wish = "";
                    $scope.greetingName = $scope.userInfo.Nik == null || $scope.userInfo.Nik == "" ? $scope.userInfo.Name : $scope.userInfo.Nik;
                    $scope.greeting = $scope.greetingName + ", приветствуем!";
                }
            });
    };

    $scope.getNews = () => {
        $http({ method: "POST", url: "GetNews" })
            .then((r) => {
                if (r.data.success) {
                    $scope.news = r.data.news;
                }
            });
    };

    $scope.getUserInfo();
    $scope.getNews();

    $scope.logOut = () => {

        $http({
            method: 'GET',
            url: "/Main/LogOut"
        })
    };

    $scope.sendWish = () => {
        $http({ method: "POST", url: "SendWish", data: { wish: $scope.userInfo.wish } })
            .then((r) => {
                if (r.data.success) {
                    $scope.wishSended = true;
                    $scope.userInfo.wish = "";
                }
            });
    };

});