from selenium import webdriver
from data.config import settings
from urllib.parse import urljoin
import allure
from allure_commons.types import AttachmentType
from selenium.webdriver.chrome.options import Options 

class WebApp:
    instance = None

    @classmethod
    def get_instance(cls):
        if cls.instance is None:
            cls.instance = WebApp()
        return cls.instance

    def __init__(self):
        if str(settings['browser']).lower() == "firefox":
            self.driver = webdriver.Firefox()
        elif str(settings['browser']).lower() == "chrome":
            self.driver = webdriver.Chrome()
        elif str(settings['browser']).lower() == "chromeheadless":
            chrome_options = Options()  
            chrome_options.add_argument("--headless")  
            self.driver = webdriver.Chrome(chrome_options=chrome_options)
        else:
            self.driver = webdriver.Firefox()

    def get_driver(self):
        return self.driver

    def load_website(self):
        self.driver.get(settings['url'])

    def goto_page(self, page):
        self.driver.get(urljoin(settings['url'], page.lower()))
        allure.attach(self.driver.get_screenshot_as_png(), name='pageImage.png',
                      attachment_type=allure.attachment_type.PNG)

    def verify_component_exists(self, component):
        # Simple implementation
        assert component in self.driver.find_element_by_tag_name('body').text, \
            "Component {} not found on page".format(component)

    def make_screenshot(self):
        # Simple implementation
        allure.attach(self.driver.get_screenshot_as_png(), name='pageImage.png',
                      attachment_type=allure.attachment_type.PNG)

webapp = WebApp.get_instance()
