(function () {
    appModule.controller('tenant.views.OldList.modalTransferProfessionnalFee', [
        '$scope',
        '$uibModalInstance',
        'abp.services.app.cause',
        '$uibModal',
        'input',
        'abp.services.app.timeSheet',
        'abp.services.app.employee',
        function (
            $scope,
            $uibModalInstance,
            causeService,
            $uibModal,
            input,
            timesheetService,
            employeeService
            ) {
            var vm = this;
            vm.ts = input;
            vm.data = { tid: input,name:""};
            vm.saving = false;
            vm.item = {};

            vm.save = function () {
                console.log('saving', vm.ts.listProFee);
                /*
                vm.ts.listProFee.forEach(function (element) {
                    console.log(element);
                }); */
                var clone = $.extend({}, vm.ts.listProFee);
                var listProFee = $.map(clone, function (n, i) {
                    //console.log('mapping', n);
                    var profee = { professionalFeeID: n.professionalFeeID, chargedBy: n.chargedBy, chargedFeePerHour: n.chargedFeePerHour };
                    if (!n.chargedBy) {
                        profee.chargedBy = n.chargedBy;
                    }
                    if (!n.chargedFeePerHour) {
                        profee.chargedFeePerHour = n.chargedFeePerHour;
                    }
                    return profee;
                });
                //console.log(listProFee);
                var postdata = $.extend({}, { timeSheetID: vm.ts.timeSheetID, listProFee: listProFee });
                console.log('saving postdata', postdata);
                timesheetService.transferTimesheet(postdata)
                    .then(function (result) {
                        if (result.data == 'ok') {
                            abp.notify.info('The Time Sheet has been saved successfully');
                        }
                        //$uibModalInstance.close();                        
                    })
                    .finally(function () {
                        vm.loading = false;
                        init();
                    });
            };            

            vm.changeProfessional = function (selectEm, selectTask) {
                //console.log('Task change', vm.ts.listProFee[selectTask]);
                //console.log('Change Employee', selectEm);
                var a;
                for (i = 0; i < vm.assignedList.length; i++) {
                    if (vm.assignedList[i].employeeID == selectEm) {
                        a = vm.assignedList[i];
                    }
                }
                //console.log('Charge Time', vm.ts.listProFee[selectTask].approveTime);
                //console.log('New fee per hour', a.fee);
                var proindex = vm.ts.listProFee.indexOf(selectTask);
                vm.ts.listProFee[proindex].chargedBy = selectEm;
                vm.ts.listProFee[proindex].chargedFeePerHour = a.fee;
            };

            vm.getAssignedEmployee = function (timesheetId) {
                console.log('employeeService.getEmployeeListByTS timesheetId', timesheetId);
                employeeService.getEmployeeListByTS(timesheetId)
                .then(function (result) {
                    vm.assignedList = result.data;
                    console.log('employeeService.getEmployeeListByTS', vm.assignedList);
                });
            };

            vm.cancel = function () {
                $uibModalInstance.close();
                console.log('Closing Transfer TimeSheet', vm.ts.timeSheetID);
            };

            function init() {
                //console.log('timesheet.input', vm.ts);
                vm.getAssignedEmployee(vm.ts.timeSheetID);
            }
            init();
        }
    ]);
})();