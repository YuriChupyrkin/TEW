import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConstantStorage } from './constantStorage'
import { PubSub } from './pubSub';

import 'rxjs/add/operator/map';

@Injectable()
export class HttpService {

    constructor(private http: Http) { 
    }

    public processGet<T>(url: string, isExternalRequest = false): Promise<T> {
        let headers = new Headers();
        let userId = ConstantStorage.getUserId()

        if (isExternalRequest == false && userId) {
            headers.append('Authorization', userId.toString());
        }

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        var getRequest = this.http.get(url, { headers: headers}).map(response => <T>response.json());
        //return getRequest;

        let promise = new Promise<T>((resolve, reject) => {
            getRequest.subscribe(
                r => {
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    resolve(r);
                }, 
                e => { 
                    console.log(`${url} (GET): request finished with error:`);
                    console.log(e);
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    reject(e);
                }
            );

        });
  
        return promise;
    }

    public processPost<T>(object: T, url: string) {
        let headers = new Headers();

        let userId = ConstantStorage.getUserId()
        if (userId) {
            headers.append('Authorization', userId.toString());
        }

        // start loading...
        PubSub.Pub(ConstantStorage.getLoadingEvent(), true);

        var postRequest = this.http.post(url, object, { headers: headers });

        //postRequest.subscribe(r => this.requestFinished(), e => this.requestFinishedWithError(url, 'post', e));
        //return postRequest;

         
        let promise = new Promise((resolve, reject) => {
            postRequest.subscribe(
                r => {
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    resolve(r);
                }, 
                e => { 
                    console.log(`${url} (POST): request finished with error:`);
                    console.log(e);
                    PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
                    reject(e);
                }
            );

        });
  
        return promise;
    }

    // private requestFinished() {
    //     // end loading... 
    //     PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
    // }

    // private requestFinishedWithError(url: string, method: string, error: any) {
    //     console.log(`${url} (${method}): request finished with error:`);
    //     console.log(error);

    //     // end loading...
    //     PubSub.Pub(ConstantStorage.getLoadingEvent(), false);
    // }
}