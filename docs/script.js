function MyCtrl($scope) {
    $scope.list = data;
    $scope.description = description;
    $scope.includeLegacy = false;
    
    $scope.legacyFilter = function (profile) {
      return $scope.includeLegacy || profile.supportedByVisualStudio2013;
    };
}