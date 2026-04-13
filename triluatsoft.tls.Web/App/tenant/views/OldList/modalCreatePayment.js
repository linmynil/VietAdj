(function () {
    appModule.controller('tenant.views.OldList.modalCreatePayment', [
        '$scope',
        '$uibModalInstance',
        'abp.services.app.payment',
        '$uibModal',
        'item',        
        function (
            $scope,
            $uibModalInstance,
            paymentService,
            $uibModal,
            item
            ) {
            var vm = this;

            vm.saving = false;
            vm.item = item;
            vm.newitem = {};
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                $('#paymentDate').daterangepicker({
                    singleDatePicker: true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    }
                });
            });


            vm.save = function () {
                if (vm.newitem.paymentAMT > vm.item.remainAMT) {
                    abp.notify.warn('Payment amount is larger than remain amount.');
                } else {
                    let paymentDate = moment(vm.paymentDate, 'DD-MM-YYYY').format();
                    vm.newitem.paymentDate = paymentDate;
                    vm.newitem.liabilitiesID = vm.item.liabilitiesID;
                    console.log('save', vm.newitem);
                    vm.saving = true;
                    paymentService.create(vm.newitem)
                        .then(function (result) {
                            if (result.data == 'ok') {
                                abp.notify.info('New transaction successfully created.');
                                $scope.$parent.vm.getAll();
                            } else {
                                abp.notify.warn(result.data);
                            }
                            $uibModalInstance.close();
                        }).finally(function () {
                            vm.saving = false;
                        });
                }                
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                //console.log('Invoice: ', vm.item);
                var PVCode = moment(new Date()).format("YYYYMMDDhhmmss");
                vm.newitem.paymentCode = "PV_" + PVCode;
                vm.newitem.refCode = vm.item.claimID;
                vm.newitem.paymentAMT = vm.item.remainAMT;
            }
            init();
            

        }
    ]);
})();