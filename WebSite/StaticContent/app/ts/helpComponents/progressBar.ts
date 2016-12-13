import { Component, Input  } from '@angular/core';

@Component({
    selector: 'progress-bar',
    templateUrl: '../../StaticContent/app/templates/helpComponents/progressBar.html'
})

export class ProgressBar {
    private currentProgress: number;
   
    @Input() set progress(value) {
        this.setProgress(value);
    }

    constructor() {
        this.setProgress(5);
    }

    public setProgress(progress: number) {
        progress = this.validateProgress(progress);
        this.currentProgress = progress;
        console.log(`setProgress: ${progress}`);
    }

    private  validateProgress(progress: number): number {
        if (progress > 100){
            return 100;
        }

        if (progress < 0){
            return 0;
        }

        return progress;
    }
}