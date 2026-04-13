(function () {
    appModule.controller('tenant.views.OldList.viewUserContribution', [
        '$scope',
        '$uibModal',
        'abp.services.app.employee',
        'abp.services.app.dashBoard',
        function (
            $scope,
            $uibModal,
            employeeService,
            dasBoardService
            ) {
            var vm = this;
            vm.listEmps = [];
            vm.page = 1;
            vm.hide = true;
            vm.hide1 = true;
            vm.listUserContribution = [];

            vm.isAdmin = false;

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            //vm.dateRangeOptions.minDate = vm.isAdmin ? new Date(2000, 1, 1) : new Date(2017, 1, 1);
            vm.dateRangeModel = {
            };


            vm.getEmployees = function () {
                    employeeService.getEmployeesForCRS({})
                        .then(function (result) {
                            vm.listEmps = result.data;                            
                        });
            };

            vm.checkAdminRole = function () {
                dasBoardService.isAdminCurrentUser({})
                    .then(function (result) {                        
                        vm.isAdmin = result.data;                        
                        vm.dateRangeOptions.minDate = vm.isAdmin ? new Date(2000, 1, 1) : new Date(2017, 1, 1);
                    });
            };

            vm.init = function () {
                //vm.getAll();
                vm.checkAdminRole();                
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
                vm.userContributionLoading = true;
                employeeService.getUserContributionInfo(postdata)
                    .then(function (result) {
                        console.log('result', result.data);
                        vm.listUserContribution = result.data;
                    })
                    .finally(function () {
                        vm.userContributionLoading = false;
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
                
                employeeService.exportUserContribution(postdata)
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
            };
        }
    ]);
})();