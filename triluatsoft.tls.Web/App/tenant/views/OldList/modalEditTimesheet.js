(function () {
    appModule.controller('tenant.views.OldList.modalEditTimesheet', [
        '$scope',
        '$uibModalInstance',
        '$uibModal',
        'timesheet',
        'abp.services.app.timeSheet',
        'abp.services.app.employee',
        'abp.services.app.dashBoard',
        function (
            $scope,
            $uibModalInstance,
            $uibModal,
            timesheet,
            timesheetService,
            employeeService,
            dashboardService
        ) {
            var vm = this;
            //vm.data = [];
            vm.query = "";
            vm.isAdmin = false;
            vm.saving = false;
            vm.currentEx = 1;
            vm.currentDis = 0;
            vm.timesheet = timesheet;
            var data = vm.timesheet.tid;

            vm.loading = true;

            vm.grant_transfer = false;
            vm.grant_issue = false;
            vm.grant_submit = false;
            vm.grant_invoice = false;

            vm.save = function () {
                //console.log('saving', vm.timesheet.listProFee);
                //vm.timesheet.listProFee.forEach(function (element) {
                //    console.log(element);
                //});
                var clone = $.extend({}, vm.timesheet.listProFee);
                var listProFee = $.map(clone, function (n, i) {
                    //console.log('mapping', n);
                    var profee = { professionalFeeID: n.professionalFeeID, workTime: n.workTime, approveTime: n.approveTime };                    
                    if (!n.workTime) {
                        profee.workTime = n.workTime;
                    }
                    if (!n.approveTime) {
                        profee.approveTime = n.approveTime;
                    }
                    return profee;
                });
                //console.log(listProFee);
                var postdata = $.extend({}, { timeSheetID: vm.timesheet.timeSheetID, exchangeRate: vm.timesheet.exchangeRate, discountVal: vm.timesheet.discountVal, discountType: vm.timesheet.discountType, listProFee: listProFee });
                //console.log('saving postdata', postdata);
                timesheetService.updateTimesheet(postdata)
                    .then(function (result) {
                        if (result.data === 'ok') {
                            abp.notify.info('The Time Sheet has been saved successfully');
                        }
                        //$uibModalInstance.close();
                    })
                    .finally(function () {
                        vm.loading = false;
                        vm.getAll();
                    });
               
            };
            
            vm.OnchangeApproveTime = function (changeitem) {                                
                var proindex = vm.timesheet.listProFee.indexOf(changeitem);                
                if ((vm.timesheet.listProFee[proindex].approveTimeStr !== null) && (undefined !== vm.timesheet.listProFee[proindex].approveTimeStr)) {                    
                    if (vm.timesheet.listProFee[proindex].approveTimeStr.length === 4) {
                        vm.timesheet.listProFee[proindex].approveTime = vm.timesheet.listProFee[proindex].approveTimeStr;
                        var apprv_time = vm.timesheet.listProFee[proindex].approveTimeStr;
                        var apprv_hour = apprv_time.substring(0, 2);
                        var apprv_min = apprv_time.substring(4, 2);
                        var apprv_dec = (apprv_hour - 0) + ((apprv_min - 0) / 60);
                        var apprv_fee = apprv_dec * vm.timesheet.listProFee[proindex].feePerHour;
                        vm.timesheet.listProFee[proindex].chargedProFeeValue = apprv_fee;
                    }                    
                } 
            };
            
            vm.cancel = function () {
                $uibModalInstance.close();
            };
            

            //modal TransferProfessionnalFee
            vm.openModalTransferTimesheet = function (input) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalTransferProfessionnalFee.cshtml',
                    controller: 'tenant.views.OldList.modalTransferProfessionnalFee as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        input: function () {
                            return input;
                        }
                    }
                });
                /*
                modalInstance.result.then(function (result) {
                    init();
                }); */
                modalInstance.result.then(function () {
                    //console.log('Return from Transfer TimeSheet');
                    init();
                }, function () {
                    //console.log('Return from Transfer TimeSheet');
                    init();
                });
            };
            

            vm.setExpType = function (item) {
                console.log('setExpType', item);
                $('#ExpButton').html(item + ' <span class="caret" />');
                if (item === 'A') {
                    $('#ExpButton').html('Amount <span class="caret" />');
                }
                else {
                    $('#ExpButton').html('Percent <span class="caret" />');
                }
                vm.timesheet.discountType = item;

                if (vm.timesheet.discountType === 'P') {
                    vm.timesheet.discountAMT = (vm.timesheet.proFeeAMTVND * vm.timesheet.discountVal) / 100;
                } else {
                    vm.timesheet.discountAMT = vm.timesheet.discountVal;
                }
                vm.timesheet.proFeeGrandAMT = vm.timesheet.proFeeAMTVND - vm.timesheet.discountAMT;
                vm.timesheet.taxAMT = (vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT) / 10;
                vm.timesheet.grandAMT = vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT + vm.timesheet.taxAMT;
            };

            vm.discountChange = function () {
                vm.timesheet.discountVal = vm.currentDis;
                if (vm.timesheet.discountType === 'P') {
                    vm.timesheet.discountAMT = (vm.timesheet.proFeeAMTVND * vm.timesheet.discountVal) / 100;                    
                } else {
                    vm.timesheet.discountAMT = vm.timesheet.discountVal;                    
                }
                vm.timesheet.proFeeGrandAMT = vm.timesheet.proFeeAMTVND - vm.timesheet.discountAMT;
                vm.timesheet.taxAMT = (vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT) / 10;
                vm.timesheet.grandAMT = vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT + vm.timesheet.taxAMT;
            };

            vm.changeSelectedEmployee = function (empID) {                
                if (undefined !== empID && empID !== null) {
                    vm.loading = true;
                    timesheetService.getTSInfoByEmp(data, empID)
                        .then(function (result) {
                            vm.timesheet = result.data;
                            vm.setExpType(vm.timesheet.discountType);
                            vm.currentEx = vm.timesheet.exchangeRate;
                            vm.currentDis = vm.timesheet.discountVal;                            
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                } else {
                    vm.getAll();
                }
            };

            vm.check_transfer_permission = function () {                
                timesheetService.checkPermission_TransferTimeSheet()
                .then(function (result) {
                    vm.grant_transfer = result.data;                    
                });
            };

            vm.check_issue_permission = function () {                
                timesheetService.checkPermission_IssueTimeSheet()
                .then(function (result) {
                    vm.grant_issue = result.data;                    
                });
            };

            vm.check_submit_permission = function () {                
                timesheetService.checkPermission_SubmitTimeSheet()
                .then(function (result) {
                    vm.grant_submit = result.data;                    
                });
            };

            vm.check_invoice_permission = function () {                
                timesheetService.checkPermission_InvoiceTimeSheet()
                .then(function (result) {
                    vm.grant_invoice = result.data;                    
                });
            };

            vm.check_isAdmin = function () {
                console.log('Checking Admin');
                dashboardService.isAdminCurrentUser()
                .then(function (result) {
                    vm.isAdmin = result.data;
                });
            };

            vm.getAll = function () {
                vm.loading = true;                
                timesheetService.getTSInfo(data)
                    .then(function (result) {
                        vm.timesheet = result.data;
                        //vm.data = [...new Set(vm.timesheet.listProFee.map(item => item.chargedByName))];
                        //console.log(vm.data);
                        //console.log('timesheetService.getTSInfo result', vm.timesheet);
                        vm.setExpType(vm.timesheet.discountType);
                        vm.currentEx = vm.timesheet.exchangeRate;
                        vm.currentDis = vm.timesheet.discountVal;
                        //console.log('vm.timesheet.timeSheetID', vm.timesheet.timeSheetID);
                        //vm.getAssignedEmployee(vm.timesheet.timeSheetID);                        
                    })
                    .finally(function () {
                        vm.check_isAdmin();
                        vm.loading = false;
                    });
            };

            vm.changeRateExchange = function () {
                vm.timesheet.actualFeeVND = vm.timesheet.actualFeeVND * vm.currentEx / vm.timesheet.exchangeRate;
                vm.timesheet.proFeeAMTVND = vm.timesheet.proFeeAMTVND * vm.currentEx / vm.timesheet.exchangeRate;
                if (vm.timesheet.discountType === 'P') {                    
                    vm.timesheet.proFeeGrandAMT = vm.timesheet.proFeeAMTVND - (vm.timesheet.proFeeAMTVND * vm.timesheet.discountVal)/100;
                } else {
                    vm.timesheet.proFeeGrandAMT = vm.timesheet.proFeeAMTVND - vm.timesheet.discountVal;
                }
                vm.timesheet.taxAMT = (vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT)/10;
                vm.timesheet.grandAMT = vm.timesheet.proFeeGrandAMT + vm.timesheet.expenseAMT + vm.timesheet.taxAMT;
                vm.timesheet.exchangeRate = vm.currentEx;
            };
            
            vm.getAssignedEmployee = function (timesheetId) {
                //console.log('employeeService.getEmployeeListByTS timesheetId', timesheetId);
                employeeService.getEmployeeListByTS(timesheetId)
                .then(function (result) {
                    vm.assignedList = result.data;
                    //console.log('employeeService.getEmployeeListByTS', vm.assignedList);
                });
            };

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
                    init();
                });
            };
            //Message Un-Issue
            vm.unissue = function (timesheetId) {
                vm.saving = true;
                timesheetService.unIssued(timesheetId)
                    .then(function (result) {
                        console.log('unissue result', result.data);
                        if (result.data === 'ok') {
                            abp.message.success(app.localize('TheTimesheethasbeenunissuedsuccessfully'));
                            init();
                        } else {
                            abp.notify.error(result.data);
                        }
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            //Modal Index ModalIndexInvoice
            vm.openModalCreateInvoice = function (input) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/modalIndexCreateInvoice.cshtml',
                    controller: 'tenant.views.OldList.modalIndexCreateInvoice as vm',
                    backdrop: 'static',
                    size: 'lg',
                    scope: $scope,
                    resolve: {
                        input: function () {
                            return input;
                        }
                    }
                });
                modalInstance.result.then(function (result) {

                });
            };

            //Message Submit
            vm.isSubmit = function (timesheetId) {
                vm.saving = true;
                timesheetService.isSubmit(timesheetId)
                    .then(function (result) {
                        //console.log('unissue result', result.data);
                        if (result.data === 'ok') {
                            abp.message.success(app.localize('thetimesheethasbeensubmittessuccessfully'));
                            init();
                        } else {
                            abp.notify.error(result.data);
                        }
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            //Message Un-submit
            vm.unSubmit = function (timesheetId) {
                vm.saving = true;
                timesheetService.unIsSubmit(timesheetId)
                    .then(function (result) {
                        //console.log('unissue result', result.data);
                        if (result.data === 'ok') {
                            abp.message.success(app.localize('thetimesheethasbeensubmittesunsuccessfully'));
                            init();
                        } else {
                            abp.notify.error(result.data);
                        }
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            function init() {
                vm.getAll();
                vm.check_transfer_permission();
                vm.check_issue_permission();
                vm.check_submit_permission();
                vm.check_invoice_permission();
                vm.getAssignedEmployee(data);
                vm.check_isAdmin();
            }
            init();

        }
    ]);
})();