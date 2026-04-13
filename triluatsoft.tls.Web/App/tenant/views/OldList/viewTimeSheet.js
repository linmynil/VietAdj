(function () {
    appModule.controller('tenant.views.OldList.viewTimeSheet', [
        '$scope',
        '$uibModal',
        'abp.services.app.timeSheet',
        function (
            $scope,
            $uibModal
            ,timesheetService
            ) {
            var vm = this;
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
            });

            vm.permissions = {
                createTimeSheet: abp.auth.hasPermission('Pages.TimesheetsManagement.CreateTimesheet'),
                enterTimeSheet: abp.auth.hasPermission('Pages.TimesheetsManagement.EnterMyProfFeeExpense'),
                issueTimeSheet: abp.auth.hasPermission('Pages.TimesheetsManagement.IssueMyTimesheet'),
                deleteTimeSheet: abp.auth.hasPermission('Pages.TimesheetsManagement.DeleteTimesheet'),
                editAM: abp.auth.hasPermission('Pages.TimesheetsManagement.EditAMTimesheet'),
                editAE: abp.auth.hasPermission('Pages.TimesheetsManagement.EditMyTimesheet'),
                editAll: abp.auth.hasPermission('Pages.TimesheetsManagement.EditTimesheet'),
            };

            vm.page = 1;
                                    
            vm.grant_create = false;
            vm.grant_enter = false;
            vm.grant_issue = false;
            vm.grant_delete = false;
            vm.grant_edit = false;

            //vm.permissions = {
            //    editAM: abp.auth.hasPermission('Pages.TimesheetsManagement.EditAMTimesheet'),
            //    editAE: abp.auth.hasPermission('Pages.TimesheetsManagement.EditMyTimesheet'),
            //    editAll: abp.auth.hasPermission('Pages.TimesheetsManagement.EditTimesheet'),
            //};

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {
            };

            vm.init = function () {                
                vm.getAll();
                vm.check_create_permission();
                //vm.check_enter_permission();
                //vm.check_issue_permission();
                //vm.check_delete_permission();
                //vm.check_edit_permission();
            }
            
            vm.check_create_permission = function () {
                console.log('Check permission for create timesheet');
                if (!vm.permissions.createTimeSheet) {
                    document.getElementById("createbtn").disabled = true;
                } else {
                    document.getElementById("createbtn").disabled = false;
                }
                //timesheetService.checkPermission_CreateTimeSheet()
                //.then(function (result) {
                //    vm.grant_create = result.data;                    
                //    console.log('Create timesheet Permission', vm.grant_create);
                //    if (!vm.grant_create) {
                //        document.getElementById("createbtn").disabled = true;
                //    } else {
                //        document.getElementById("createbtn").disabled = false;
                //    }
                //});
            };
            
            vm.check_enter_permission = function () {
                console.log('Check permission for enter timesheet');
                timesheetService.checkPermission_EnterTimeSheet()
                .then(function (result) {
                    vm.grant_enter = result.data;
                    console.log('Enter TimeSheet Permission', vm.grant_enter);
                });
            };

            vm.check_issue_permission = function () {
                console.log('Check permission for issue timesheet');
                timesheetService.checkPermission_IssueTimeSheet()
                .then(function (result) {
                    vm.grant_issue = result.data;
                    console.log('Issue TimeSheet Permission', vm.grant_issue);
                });
            };
            
            vm.check_delete_permission = function () {
                console.log('Check permission for delete timesheet');
                timesheetService.checkPermission_DeleteTimeSheet()
                .then(function (result) {
                    vm.grant_delete = result.data;
                    console.log('Delete TimeSheet Permission', vm.grant_delete);
                });
            };

            vm.check_edit_permission = function () {
                console.log('Check permission for edit timesheet');
                timesheetService.checkPermission_EditTimeSheet()
                .then(function (result) {
                    vm.grant_edit = result.data;
                    console.log('Edit TimeSheet Permission', vm.grant_edit);
                });
            };

            vm.getAll = function () {                
                console.log('request params', $.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }));
                console.log('Permission for TimeSheet: ', vm.permissions);
                vm.loading = true;
                timesheetService.searchTimeSheet($.extend({}, vm.searchdata, vm.dateRangeModel, { page: vm.page }))
                    .then(function (result) {
                        vm.list = result.data.items;
                        console.log('list', vm.list);
                        vm.totalCount = result.data.totalCount;
                    })
                    .finally(function () {
                        vm.loading = false;
                    });
            };

            vm.pageChanged = function () {
                vm.init();                
            };

            vm.search = function (event) {
                try {
                    vm.getAll();
                } catch (e) {
                    console.log(e)
                } finally {
                    event.preventDefault();
                }
            }

            vm.reset = function () {
                console.log('vm.reset');
                vm.searchdata = {};
                vm.dateRangeModel = {};
                vm.page = 1;
            }

            //modal CreateTimeSheet
            vm.openModalCreateTimeSheet = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalAddTimeSheet.cshtml',
                    controller: 'tenant.views.OldList.modalAddTimeSheet as vm',
                    backdrop: 'static',
                    scope: $scope
                });

                modalInstance.result.then(function (result) {
                    vm.init();
                });
            }

            //Modal Index ModalIndexFeeExpenses
            vm.openModalIndexTimeSheet = function (timesheetId, timeSheetName) {
                var innerTimeSheetID = timesheetId;
                var innerTimeSheetName = timeSheetName;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalIndexFeeExpenses.cshtml',
                    controller: 'tenant.views.OldList.modalIndexFeeExpenses as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        timesheetId: function () {
                            return innerTimeSheetID;
                        }
                        , timesheetName: function () {
                            return innerTimeSheetName;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    console.log('close modal fee', result);
                    vm.init();
                });
            }

            //modal EditTimeSheet
            vm.openModalDetailTimesheet = function (timesheetParam) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalDetailTimeSheet.cshtml',
                    controller: 'tenant.views.OldList.modalDetailTimeSheet as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        timesheet: function () {
                            return timesheetParam;
                        }
                    }
                });


                modalInstance.result.then(function (result) {
                    vm.init();
                });
                
            }
            //modal EditTimeSheet
            vm.openModalEditTimesheet = function (timesheetParam) {
                var en_edit = false;
                if (!vm.permissions.editAll) {
                    if (vm.permissions.editAM && timesheetParam.tsRole === 1) {
                        //Edit AE
                        en_edit = true;
                    } else {
                        if (vm.permissions.editAE && (timesheetParam.tsRole === 2 || timesheetParam.tsRole === 1)) {
                            //Edit AM
                            en_edit = true;
                        }
                    }                    
                } else {
                    en_edit = true;
                }

                if (!en_edit) {                    
                    abp.notify.error(app.localize('NotPermissionEditTimeSheet'));
                } else {
                    console.log('open modal edit timesheet');
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/tenant/views/OldList/modalEditTimesheet.cshtml',
                        controller: 'tenant.views.OldList.modalEditTimesheet as vm',
                        backdrop: 'static',
                        size: 'lg',
                        resolve: {
                            timesheet: function () {
                                return timesheetParam;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        vm.init();
                    }, function () {
                        vm.init();
                    });
                }                
            }

            //Message Issue
            vm.openModalIssue = function (timesheetId, timeSheetName) {
                console.log('open modal issue', timesheetId, timeSheetName);
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalIssue.cshtml',
                    controller: 'tenant.views.OldList.modalIssue as vm',
                    backdrop: 'static',
                    resolve: {
                        timesheetId: function () {
                            return timesheetId;
                        }
                        , timesheetName: function () {
                            return timeSheetName;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    vm.getAll();
                });
            }
            //Message Un-Issue
            vm.unissue = function (timesheetId) {
                vm.saving = true;
                timesheetService.unIssued(timesheetId)
                    .then(function (result) {
                        console.log('unissue result', result.data);
                        if (result.data === 'ok') {
                            abp.message.success(app.localize('TheTimesheethasbeenunissuedsuccessfully'));
                            vm.getAll();
                        } else {
                            abp.notify.error(result.data);
                        }
                }).finally(function () {
                    vm.saving = false;
                });
            }
            //Message Delete
            vm.deleteTimeSheet = function (timesheetId) {
                abp.message.confirm('',
                    function (isConfirmed) {
                       if (isConfirmed) {
                           console.log('delete timesheetId', timesheetId);
                           timesheetService.deleteTimeSheet(timesheetId)
                               .then(function (result) {
                                   abp.notify.info(result.data);
                               })
                               .then(function () {
                                   vm.getAll();
                               });
                        }
                       });
            }


            vm.init();

        }
    ]);
})();