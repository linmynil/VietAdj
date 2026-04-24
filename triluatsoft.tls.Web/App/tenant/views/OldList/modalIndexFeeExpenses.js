(function () {
    appModule.controller('tenant.views.OldList.modalIndexFeeExpenses', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'timesheetId',
        'timesheetName',
        'abp.services.app.timeSheet',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            timesheetId,
            timesheetName,
            timesheetService
        ) {
            var vm = this;
            vm.list = [];
            vm.timesheetId = timesheetId;
            vm.timesheetName = timesheetName;
            

            $scope.del = function (i) {
                console.log(i);
                $scope.items.splice(i, 1);
            }
            $scope.selectTab = function (setTab) {

                this.tab = setTab;
            };
            $scope.isSelected = function (checkTab) {
                return this.tab === checkTab;
            };

            vm.save = function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
            };

            vm.cancel = function () {
                $uibModalInstance.close();
            };

            //modal Free
            vm.openModalAddFee = function (itemIn) {
                console.log('open openModalAddFee', itemIn);
                vm.item = itemIn;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalFee.cshtml',
                    controller: 'tenant.views.OldList.modalFee as vm',
                    scope: $scope, //Refer to parent scope here
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            //return vm.item;
                            return angular.copy(itemIn);
                        },
                        recordType: function () {
                            return vm.recordType;
                        },
                        timesheetName: function () {
                            return vm.timesheetName;
                        },
                        timesheetId: function () {
                            return vm.timesheetId;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    console.log('modal add fee done');
                });
            };  
            //END

            //modal Expenses
            vm.openModalExpenses = function (itemIn1) {
                console.log('open modalExpenses', itemIn1);
                vm.item = itemIn1;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalExpenses.cshtml',
                    controller: 'tenant.views.OldList.modalExpenses as vm',
                    scope: $scope, //Refer to parent scope here
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.item;
                        },
                        timesheetName: function () {
                            return vm.timesheetName;
                        },
                        timesheetId: function () {
                            return vm.timesheetId;
                        }
                    }
                });
                modalInstance.result.then(function () {
                });
            };
            //end

            

            function init() {
            }
            init(); 

            //Message Delete
            vm.removeFee = function (proFeeId) {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            console.log('delete fee id', proFeeId);
                            timesheetService.deleteFee(proFeeId)
                                .then(function (result) {
                                    console.log('delete fee result', result);
                                })
                                .then(function () {
                                    vm.getAll();
                                });
                        }
                    });
            }
            //Message Delete
            vm.removeEx = function (expenseId) {
                abp.message.confirm('AreYouSureDeleteInformation?',
                    function (isConfirmed) {
                        if (isConfirmed) {
                            console.log('delete expenseId', expenseId);
                            timesheetService.deleteExpense(expenseId)
                                .then(function (result) {
                                    console.log('delete expenseId result', result);
                                })
                                .then(function () {
                                    vm.getAll();
                                });
                        }
                    });
            }




            vm.getAll = function () {
                vm.loading = true;
                console.log('init request', timesheetId);
                timesheetService.getProfessionalFee(timesheetId)
                    .then(function (result) {
                        vm.list = result.data;
                        console.log('GetProfessionalFee', vm.list);
                    })
                    .finally(function () {
                        vm.loading = false;
                    });

                timesheetService.getUserExpense(timesheetId)
                    .then(function (result) {
                        vm.listExp = result.data;
                        console.log('getUserExpense', vm.listExp);
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            vm.getAll();
        }
    ]);
})();