#include <iostream>
#include <vector>
#include <fstream>
#include <algorithm>
#include <stdexcept>
#include <cassert>

using namespace std;

//Поганий приклад
int main(){int x=10;if(x>0){cout<<"A positive number:"<<endl;}}

//Гарний приклад
int main()
{
    int x = 10;
    if (x > 0)
    {
        cout << "A positive number: " << x << endl;
    }
    return 0;
}

//Поганий приклад
class class1 {
    int a = 25;

    bool f() {
        return a >= 18;
    }
};

//Гарний приклад
class UserAccount {
    int user_age = 25;

    bool isAdult() {
        return user_age >= 18;
    }
};

//Гарний приклад
// rectangle.h
class Rectangle {
public:
    Rectangle(int width, int height);
    int calculateArea() const;

private:
    int width;
    int height;
};

// rectangle.cpp
#include "rectangle.h"

Rectangle::Rectangle(int width, int height) : width(width), height(height) {}

int Rectangle::calculateArea() const {
    return width * height;
}

//Поганий приклад
class UserAccount { // Оголошення класу UserAccount
    int user_age = 25; // Оголошення змінної user_age

    bool isAdult() { //Оголошення функції isAdult
        return user_age >= 18; //Повернення результату
    }
};

//Гарний приклад
//TODO: Додати метод для оновлення віку користувача
class UserAccount {
    int user_age = 25;

    bool isAdult() {
        return user_age >= 18;
    }
};

//Поганий приклад
void processAndLogData(const vector<int>& data) {
    for (int value : data) {
        cout << "Processing: " << value << endl;
        ofstream log("data.txt", ios_base::app);
        log << "Processed: " << value << endl;
    }
}

//Гарний приклад
void processData(const vector<int>& data) {
    for (int value : data) {
        cout << "Processing: " << value << endl;
        logData(value);
    }
}

void logData(int value) {
    ofstream log("data.txt", ios_base::app);
    log << "Processed: " << value << endl;
}

//Поганий приклад
int addNumbers1(int a) {
    return a + 1;
}

int addNumbers2(int a) {
    return a + 2;
}

//Гарний приклад
int addNumbers(int a, int b) { 
    return a + b; 
}

//Поганий приклад
void processVector(vector<int> data) {
    bool swapped;
    do {
        swapped = false;
        for (size_t i = 1; i < data.size(); ++i) {
            if (data[i - 1] > data[i]) {
                swap(data[i - 1], data[i]);
                swapped = true;
            }
        }
    } while (swapped);
}

//Гарний приклад
void processVector(const vector<int>& data) {
    vector<int> sortedData = data;
    sort(sortedData.begin(), sortedData.end());
}

//Поганий приклад
int divideNumbers(int a, int b) {
    return a / b;
}

//Гарний приклад
int divideNumbers(int a, int b) {
    if (b == 0) {
        throw invalid_argument("Error: Division by zero");
    }
    return a / b;
}

int main() {
    try {
        int result = divideNumbers(10, 0);
    }
    catch (const invalid_argument& e) {
        cerr << "Caught exception: " << e.what() << endl;
    }
}

/**
 * @brief Клас калькулятора для базових математичних операцій
 */
class Calculator {
public:
    /**
     * @brief Ділення двох чисел
     * @param a Число, що ділиться
     * @param b Число-дільник
     * @return Результат ділення
     * @throw invalid_argument Якщо відбувається ділення на нуль
     */
    double divide(double a, double b) {
        if (b == 0) {
            throw invalid_argument("Division by zero is prohibited");
        }
        return a / b;
    }
};

class CalculatorTest {
private:
    Calculator calc;

    void testDivision() {
        assert(calc.divide(6, 2) == 3.0);
        assert(calc.divide(5, 2) == 2.5);

        try {
            calc.divide(10, 0);
            cout << "Error: Uncaught exception when dividing by zero" << endl;
        }
        catch (const invalid_argument& e) {
            cout << "Exception successfully caught: " << e.what() << endl;
        }
    }
};
