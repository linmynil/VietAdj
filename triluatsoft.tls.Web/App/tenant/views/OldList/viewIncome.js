(function () {
    appModule.controller('tenant.views.OldList.viewIncome', [
        '$scope',
        '$uibModal',
        'FileUploader',
        'abp.services.app.employeeIncome',
        'abp.services.app.employee',
        'abp.services.app.dashBoard',
        function (
            $scope,
            $uibModal,
            fileUploader,
            employeeIncomeService,
            employeeService,
            dasBoardService
            ) {
            var vm = this;
            vm.page = 1;
            vm.typelist = {};
            vm.listEmps = {};
            vm.searchdata = {};
            vm.searchdata.contribution = '';
            vm.incomeList = {};

            vm.viewby = 20;
            vm.totalItems = 0;
            vm.currentPage = 1;
            vm.itemsPerPage = vm.viewby;
            vm.maxSize = 10; //Number of pager buttons to show

            vm.isAdmin = false;

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };                                    

            vm.uploadedFileName = null;
            //vm.submitting = true;

            vm.uploader = new fileUploader({                                
                url: abp.appPath + 'File/UploadFile',
                headers: {
                    "X-XSRF-TOKEN": abp.security.antiForgery.getToken()
                },
                queueLimit: 1,
                autoUpload: true,
                removeAfterUpload: true,
            });

            vm.uploader.onSuccessItem = function (fileItem, response, status, headers) {                                
                if (response.success) {
                    vm.uploadedFileName = response.result.fileName;                                        
                } else {
                    abp.message.error('Error: ', response.error.message);                    
                }
            };

            vm.UploadFile = function(event) {
                event.preventDefault();                
                if (!vm.uploadedFileName) {
                    return;
                }                                
                employeeIncomeService.importIncomeExcel(vm.uploadedFileName).then(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));                    
                    vm.init();
                    document.getElementById("uploadFile").value = "";
                });
            };

            vm.getTypeList = function () {                
                employeeIncomeService.getAllIncomeType()
                    .then(function (result) {
                        vm.typelist = result.data;                        
                    });
            };

            vm.getEmployees = function () {
                employeeService.getEmployeeListIncome({})
                    .then(function (result) {
                        vm.listEmps = result.data;
                        console.log('Employee list: ', vm.listEmps);
                    });
            };

            vm.getPostData = function () {
                let postdata = vm.searchdata;
                if (postdata.employeeID == undefined) {
                    postdata.employeeID = 0;
                }
                postdata.startDate = vm.dateRangeModel.startDate;
                postdata.endDate = vm.dateRangeModel.endDate;
                return postdata;
            }
            vm.getAll = function () {                                
                let postdata = vm.getPostData();
                employeeIncomeService.search(postdata)
                    .then(function (result) {
                        vm.incomeList = result.data;
                        console.log('Income Result: ', vm.incomeList);
                        vm.totalItems = vm.incomeList.length;
                    });
            };
            vm.searchLogs = function (event) {
                event.preventDefault();
                let postdata = vm.getPostData();
                console.log('Search data: ', postdata);
                employeeIncomeService.search(postdata)
                    .then(function (result) {
                        vm.incomeList = result.data;
                        vm.totalItems = vm.incomeList.length;                        
                    });
            }

            vm.checkAdminRole = function () {
                dasBoardService.isAdminCurrentUser({})
                    .then(function (result) {
                        vm.isAdmin = result.data;
                        vm.dateRangeOptions.minDate = vm.isAdmin ? new Date(2000, 1, 1) : new Date(2017, 1, 1);
                    });
            };

            vm.init = function () {
                vm.checkAdminRole();
                vm.getTypeList();
                vm.getEmployees();
                vm.getAll();
            }

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            }

            //modal CreateIncome
            vm.openModal = function (itemIn, mode) {
                console.log('open modalCreateIncome', itemIn, mode);
                vm.item = itemIn;
                vm.mode = mode;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalCreateIncome.cshtml',
                    controller: 'tenant.views.OldList.modalCreateIncome as vm',
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.item;
                        },
                        mode: function () {
                            return vm.mode;
                        },
                        recordType: function () {
                            return vm.recordType;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };
            //END
        }
    ]);
})();