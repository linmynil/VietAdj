(function () {
    appModule.controller('tenant.views.OldList.modalOtherReceipts', [
        '$scope',
        '$uibModalInstance',
        'id',
        'abp.services.app.cash',
        function (
            $scope,
            $uibModalInstance,
            id,
            cashService
        ) {
            var vm = this;

            vm.saving = false;
            vm.id = id;
            vm.item = {};
            //let date2;
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                $('#createdDate').daterangepicker({
                    singleDatePicker: true,
                    startDate: moment().startOf('day'),
                    locale: {
                        format: 'DD-MM-YYYY'
                    }
                });
                

            });

            vm.getCashById = function () {                
                console.log('vm.getCashById', vm.id);
                if (vm.id) {
                    cashService.getById(vm.id)
                        .then(function (result) {
                            console.log('getCashById result', result.data);
                            return vm.item = result.data;
                        })
                        .then(function () {

                            var startDate = moment(vm.item.createdDate);
                            console.log('startdate', startDate);
                            $('#createdDate').daterangepicker({
                                singleDatePicker: true,
                                startDate: startDate.format('DD-MM-YYYY'),
                                locale: {
                                    format: 'DD-MM-YYYY'
                                }
                            });
                        });
                }
            }

            vm.save = function () {
                console.log('save vm.createdDate ', vm.createdDate);
                //vm.item.dateTimeNow = moment(date2, 'DD-MM-YYYY').format();
                vm.item.createdDate = moment(vm.createdDate, 'DD-MM-YYYY').format();
                vm.item.paymentMethod = vm.item.payment;
                vm.item.isDelete = false;
                console.log('save item', vm.item);
                let parent = $scope.$parent;                
                cashService.saveCash(vm.item)
                    .then(function (result) {
                        console.log('save result', result.data);
                        if (result.data == 'ok') {
                            abp.notify.info('New transaction successfully created');
                        }
                        parent.vm.getAll();
                    })
                $uibModalInstance.close();
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
            function init() {
                //let date1 = $('#DateTimeNow').daterangepicker({
                //    singleDatePicker: true,
                //    startDate: moment().startOf('day'),
                //    locale: {
                //        format: 'DD-MM-YYYY'
                //    }
                //});
                //date2 = date1.context.lastModified;
                //in edit mode
                if (vm.id) {
                    vm.getCashById();
                }
            }

            init();
        }
    ]);
})();