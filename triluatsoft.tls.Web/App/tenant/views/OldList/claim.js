(function () {
    appModule.controller('tenant.views.OldList.claim', [
        '$scope', '$uibModal', '$window', '$location', '$stateParams', '$state', 'abp.services.app.claim'
        , 'abp.services.app.typeOfLoss', 'abp.services.app.cause', 'abp.services.app.claimType'
        , 'abp.services.app.customer', 'abp.services.app.employee',
        function ($scope, $uibModal, $window, $location, $stateParams, $state, claimService
            , typeOfLossService, causeService, claimTypeService
            , customerService, employeeService) {
            var vm = this;
            console.log('$stateParams', $stateParams);
            var claimRefId = $stateParams.claimRefId;
            vm.searchdata = {};
            vm.searchdata.claimRefId = claimRefId;
            console.log('claimRefId', claimRefId);
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
            });
            
            vm.permissions = {
                createClaim: abp.auth.hasPermission('Pages.ClaimsManagement.CreateClaim'),
                searchClaim: abp.auth.hasPermission('Pages.ClaimsManagement.SearchClaim'),
                editClaim: abp.auth.hasPermission('pages.ClaimsManagement.EditMyClaims'),
            };

            vm.list = [];
            vm.totalCount = 0;
            vm.page = 1;

            vm.searchdata = [];

            vm.grant_create = false;
            vm.grant_search = false;
            vm.grant_edit = false;

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
                //startDate: moment().startOf('day'),
                //endDate: moment().endOf('day')
            };

            vm.check_create_permission = function () {
                console.log('Check permission for create claim');
                if (!vm.permissions.createClaim) {
                    document.getElementById("createbnt").disabled = true;
                } else {
                    document.getElementById("createbnt").disabled = false;
                }
            };

            vm.check_search_permission = function () {
                console.log('Check permission for search claim');
                claimService.checkPermission_SearchClaim()
                .then(function (result) {
                    vm.grant_search = result.data;
                    console.log('Search Permission', vm.grant_search);                    
                });
            };

            vm.check_edit_permission = function () {
                console.log('Check permission for edit claim');
                claimService.checkPermission_EditClaim()
                .then(function (result) {
                    vm.grant_edit = result.data;
                    console.log('Edit Permission', vm.grant_edit);
                });
            };

            vm.getEmployees = function () {
                console.log('getemployees');
                employeeService.getEmployeesForClaim({})
                    .then(function (result) {                        
                        vm.listEmployees = result.data;
                        console.log('Employees list', vm.listEmployees);
                    });
            };
            vm.getTypeOfLoss = function () {
                typeOfLossService.getAll({})
                    .then(function (result) {
                        vm.listTypeOfLoss = result.data;
                    });
            };
            vm.getCauses = function () {
                causeService.getAll({})
                    .then(function (result) {
                        vm.listCauses = result.data;
                    });
            };
            vm.getClaimType = function () {
                claimTypeService.getAll({})
                    .then(function (result) {
                        vm.listClaimTypes = result.data;
                    });
            };
            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                    });
                customerService.getByType('B')
                    .then(function (result) {
                        vm.listBrokers = result.data;
                    });
            };


            vm.getAll = function () {
                console.log('request params', $.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }));
                console.log('Get all claim');
                vm.loading = true;
                claimService.getAll($.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }))
                    .then(function (result) {
                        console.log('Get all claim finished!');
                        vm.list = result.data.items;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });                                
            };

            vm.pageChanged = function () {
                vm.getAll();
            };

            vm.searchLogs = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e);
                } finally {
                    event.preventDefault();
                }
            };

            vm.init = function () {
                console.log('Permission for Claim: ', vm.permissions);
                vm.check_create_permission();
                //vm.check_search_permission();
                //vm.check_edit_permission();
                vm.getTypeOfLoss();
                vm.getCauses();
                vm.getClaimType();
                vm.getCustomers();
                vm.getEmployees();
                vm.getAll();
            };

            vm.init();

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            };
            vm.popupWindow = function (claimId, lang) {
                var url = 'report?claimId=' + claimId + '&lang=' + lang;
                $window.open(url, "popup", "width=850,height=700,left=100,top=100");
            };

            vm.openModal = function (itemIn) {
                console.log('open viewClaimModal', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/viewClaimModal.cshtml',
                    controller: 'tenant.views.OldList.viewClaimModal as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        itemId: function () {
                            return vm.item.id;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            };

            vm.redirectEdit = function (itemId) {
                if (vm.permissions.editClaim) {
                    console.log('redirect', itemId);
                    $state.go("tenant.cuClaim", { 'claimId': itemId });
                } else {
                    abp.notify.info('You have no permission to edit claim!');
                }                
            };
            //modal Folder

            //modal claimFolder
            vm.openModalFolder = function (claim) {
                //try {
                    vm.item = claim;
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/tenant/views/OldList/modalClaimFoldersDetail.cshtml',
                        controller: 'tenant.views.OldList.modalClaimFoldersDetail as vm',
                        backdrop: 'static',
                        size: 'lg',
                        resolve: {
                            item: function () {
                                return vm.item;
                            }
                        }
                    });

                    modalInstance.result.then(function () {
                        vm.getAll();
                    });
                //} catch (except) {
                    //vm.dataClaim = claim;
                    //modalInstance = $uibModal.open({
                    //    templateUrl: '~/App/tenant/views/OldList/modalClaimFolders.cshtml',
                    //    controller: 'tenant.views.OldList.modalClaimFolders as vm',
                    //    backdrop: 'static',
                    //    size: 'lg',
                    //    resolve: {
                    //        claims: function () {
                    //            return vm.dataClaim;
                    //        }
                    //    }
                    //});

                    //modalInstance.result.then(function () {
                    //    vm.getAll();
                    //});
                //}
            };
            /*
            vm.openModalFolder = function (claim) {
                debugger
                vm.dataClaim = claim;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalClaimFolders.cshtml',
                    controller: 'tenant.views.OldList.modalClaimFolders as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        claims: function () {
                            return vm.dataClaim;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    vm.getAll();
                });
            }; */

            //END
        }
    ]);
})();