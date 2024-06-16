import { ApplicationRef, NgModuleRef } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

const bootstrap = async (): Promise<ApplicationRef> => {
  const moduleRef: NgModuleRef<AppModule> = await platformBrowserDynamic().bootstrapModule(AppModule);
  return moduleRef.injector.get(ApplicationRef);
};

export default bootstrap;
