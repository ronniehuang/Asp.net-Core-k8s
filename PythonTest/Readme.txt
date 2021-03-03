1. download python and install from website https://www.python.org/
2. install pytest
	pip install -U requests
	pip install -U pytest
	pip install allure-pytest
3. start test
	pytest pythondemo.py --alluredir=allure_results
4. generate report
	allure serve allure_results