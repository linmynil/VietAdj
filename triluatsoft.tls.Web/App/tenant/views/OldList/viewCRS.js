(function () {
    appModule.controller('tenant.views.OldList.viewCRS', [
        '$scope',
        '$uibModal',
        'abp.services.app.employee',
        function (
            $scope,
            $uibModal,
            employeeService
        ) {
            var vm = this;
            vm.listEmps = [];
            vm.page = 1;
            vm.hide = true;
            vm.hide1 = true;
            vm.listContribution = [];
            vm.searchdata = {};
            vm.excel = true;
                
            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };
            
            vm.getEmployees = function () {
                employeeService.getEmployeesForCRS({})
                    .then(function (result) {
                        vm.listEmps = result.data;
                    });
            };

            vm.init = function () {
                //vm.getAll();
                vm.getEmployees();                
            };

            vm.init();
            
            vm.viewContributionR = function (event) {
                vm.hide = false;
                vm.hide1 = true;
                event.preventDefault();
                let postdata = vm.getPostData();
                console.log('vm view contribution', postdata);
                vm.action = 'contribution';
                vm.contributionLoading = true;
                employeeService.getContributionInfo(postdata)
                    .then(function (result) {
                        console.log('result', result.data);
                        vm.listContribution = result.data;
                    })
                    .finally(function () {
                        vm.contributionLoading = false;
                        vm.excel=false;
                    });
            }
            vm.viewIncomeI = function (event) {
                vm.hide = true;
                vm.hide1 = false;
                event.preventDefault();
                let postdata = vm.getPostData();
                console.log('vm viewIncomeI', postdata);

                vm.action = 'income';
                vm.userIncomeLoading = true;
                employeeService.getUserIncomeInfo(postdata)
                    .then(function (result) {
                        console.log('result', result.data);
                        vm.listUserIncome = result.data;
                    })
                    .finally(function () {
                        vm.userIncomeLoading = false;
                    });
            }

            vm.exportExcel = function (event) {
                event.preventDefault();
                if (vm.action === 'contribution') {
                    vm.exportExcelContribution(event);
                }
                if (vm.action === 'income') {
                    vm.exportExcelIncome(event);
                }
            }

            vm.exportExcelContribution = function (event) {
                event.preventDefault();
                App.startPageLoading({ animate: true });
                let postdata = vm.getPostData();

                console.log('exportExcel', postdata);

                employeeService.exportContribution(postdata)
                    .then(function (result) {
                        console.log('exportExcel result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            }

            vm.getPostData = function () {
                let postdata = vm.searchdata;                
                if (postdata.employeeID == undefined) {
                    postdata.employeeID = 0;
                }
                postdata.startDate = vm.dateRangeModel.startDate;
                postdata.endDate = vm.dateRangeModel.endDate;
                return postdata;
            }

            vm.exportExcelIncome = function (event) {
                event.preventDefault();
                App.startPageLoading({ animate: true });
                let postdata = vm.getPostData();

                console.log('exportExcel', postdata);

                employeeService.exportUserIncome(postdata)
                    .then(function (result) {
                        console.log('exportUserIncome result', result.data);
                        app.downloadTempFile(result.data);
                    })
                    .finally(function () {
                        App.stopPageLoading({ animate: true });
                    });
            }
        }
    ]);
})();