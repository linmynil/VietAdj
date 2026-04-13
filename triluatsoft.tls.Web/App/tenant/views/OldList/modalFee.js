(function () {
    appModule.controller('tenant.views.OldList.modalFee', [
        '$scope',
        '$uibModalInstance',
        'item',
        'recordType',
        'abp.services.app.taskName',
        'timesheetName',
        'timesheetId',
        'abp.services.app.timeSheet',
        function (
            $scope,
            $uibModalInstance,
            item,
            recordType,
            taskNameService,
            timesheetName,
            timesheetId,
            timesheetService
        ) {
            var vm = this;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                //mapping inputdate in case edit
                if (vm.item && vm.item.inputDate) {
                    console.log('>>>>>mapping data');
                    let date = moment(vm.item.inputDate);
                    $('#date').daterangepicker({
                        singleDatePicker: true,
                        startDate: date.format('DD-MM-YYYY'),
                        endDate: date.format('DD-MM-YYYY'),
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    });
                } 
            });            

            vm.timesheetName = timesheetName;
            vm.item = {};
            if (item) {
                vm.item = item;
            }
            vm.item.timeSheetID = timesheetId;
            
            console.log('modal fee item', item);

            vm.testDate = function () {
                console.log('Selected date', vm.item.inputDate);
            };

            vm.save = function () {                
                vm.item.inputDate = moment(document.getElementById("date").value, ["DD-MM-YYYY"]).format();
                vm.item.workTime = vm.item.workTimeStr.replace(':', '');
                console.log('save mm', vm.item.inputDate);
                var postdata = $.extend({}, vm.item);
                console.log('save', postdata);
                vm.saving = true;
                timesheetService.saveFee(postdata)
                    .then(function (result) {
                        if (result.data == 'ok') {
                            abp.notify.info('Update timesheet fee successfully');
                        }
                        $uibModalInstance.close();
                        return result.data;
                    }).then(function (result) {
                        console.log('save done', $scope.$parent);
                        $scope.$parent.vm.getAll();
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.OnchangeActualTime = function () {                
                if ((vm.item.workTimeStr !== null) && (undefined !== vm.item.workTimeStr)) {
                    if (vm.item.workTimeStr.length === 5) {
                        vm.item.workTime = vm.item.workTimeStr;                        
                    }
                    if (vm.item.workTimeStr.length === 4) {
                        vm.item.workTime = vm.item.workTimeStr;
                    }
                }
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
            vm.selectTask = function () {
                console.log('vm.item.jobCodeID', vm.item.jobCodeID);
                var temp = $.grep(vm.listTasks, function (e) { return e.id == vm.item.jobCodeID; })[0];
                console.log('temp', temp);
                if (temp) {
                    vm.item.standardTime = temp.standardTime;
                    vm.item.notes = temp.name;
                }
                console.log('vm.item.standardTime', vm.item.standardTime);
                
            }
            vm.getAll = function () {
                taskNameService.getAll({})
                    .then(function (result) {
                        vm.listTasks = result.data;
                        console.log('list', vm.listTasks);                        
                    });
            };            

            function init() {
                vm.getAll();
            }

            init();

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                console.log('Start timesheetService.getTSInfo result');
                timesheetService.getTSInfo(timesheetId)
                .then(function (result) {
                    vm.timesheet = result.data;
                    console.log('timesheetService.getTSInfo result', vm.timesheet);
                    //var mind = new Date(vm.timesheet.createdDate);
                    var mind = new Date('2000-01-01');
                    console.log('min date', mind);
                    var maxd = new Date();
                    maxd.setDate(maxd.getDate() + 365);
                    console.log('max date', maxd);

                    $('#date').daterangepicker({
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                        singleDatePicker: true,
                        autoUpdateInput: false,
                        maxDate: maxd,
                        minDate: mind,                        
                    });                    

                    $('#date').on('apply.daterangepicker', function (ev, picker) {
                        $('#date').val(picker.startDate.format('DD-MM-YYYY'));
                    });
                });
            });

            //END
        }
    ]);
})();