import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class UploadImagesService {
    apiUrl = '/api/ProcessImage/';
    uploadUrl = this.apiUrl + "upload";
    downloadUrl = this.apiUrl + "textfile";

    constructor(private _http: HttpClient) { }

    uploadImages(images: FormData): Observable<any> {
        return this._http.post(this.uploadUrl, images, { observe: 'response', responseType: 'blob' })
            .pipe(map(res => {
                var today = new Date();
                var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
                var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                let data = {
                    file: new Blob([res.body], { type: res.headers.get('Content-Type') }),
                    filename: date + time + ".meta"
                }
                console.log(data)
                return data;
            }));
    }

    processTxt(textFile: FormData): Observable<any> {
        return this._http.post(this.downloadUrl, textFile, { observe: 'response', responseType: 'blob' })
            .pipe(map(res => {
                var today = new Date();
                var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
                var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                let data = {
                    file: new Blob([res.body], { type: res.headers.get('Content-Type') }),
                    filename: date + time + ".zip"
                }
                console.log(data)
                return data;
            }));;
    }
}
