import { Component, Input  } from '@angular/core';

@Component({
    selector: 'progress-bar',
    templateUrl: '../../StaticContent/app/templates/helpComponents/progressBar.html'
})

export class ProgressBar {
    private currentProgress: number;
   
    @Input() set progress(value) {
        let progress = this.validateProgress(value);
        this.currentProgress = progress;
    }

    constructor() {
        this.currentProgress = 0;
    }

    private  validateProgress(progress: number): number {
        if (progress > 100) {
            return 100;
        }

        if (progress < 0) {
            return 0;
        }

        return progress;
    }
}