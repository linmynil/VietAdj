(function () {
    appModule.controller('tenant.views.OldList.ViewLibraryDownload', [
        '$scope',
        '$uibModal',
        function (
            $scope,
            $uibModal
            ) {
            var vm = this;
            vm.page = 1;


            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };


            vm.searchLogs = function () {
            }

            vm.init = function () {
            }

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            }

            //Message Delete
            vm.deleteDownload = function () {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            //...delete user
                        }
                    });
            }

        }
    ]);
})();