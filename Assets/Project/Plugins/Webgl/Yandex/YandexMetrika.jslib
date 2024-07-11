mergeInto(LibraryManager.library, {

    LoadedExtern: function () {
        ym(97174711,'reachGoal','loaded');
    },

    TargetAdsExtern: function (id) {
        ym(97174711,'reachGoal','ads' + id);
    },

    TargetActivityExtern: function (number) {
        ym(97174711,'reachGoal','activity' + number);
    }
});