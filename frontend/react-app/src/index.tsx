import ReactDOM from 'react-dom';
import { BrowserRouter as Router, Link, Route, useHistory } from 'react-router-dom'
import Home from './components/Home';
import Login from './components/Login';
import reportWebVitals from './reportWebVitals';
import {
    QueryClient,
    QueryClientProvider,
    useQuery
} from 'react-query';
import { getIsAuthenticated } from './api/AuthentificationService';

const queryClient = new QueryClient();


const App: React.FC = () => {
    const { data: user } = useQuery('authenticated', getIsAuthenticated);
    const history = useHistory();

    const handleSubmit = async () => {
        await queryClient.clear();
        history.push('/');
    };
    return (
        <Router>
            {/* {
                user?.isAuthenticated ? (
                    <nav>
                        <Link to="/home">Home</Link>
                        <Link to="/logout">LogOut</Link>
                    </nav>
                ) : (

                    <nav>
                        <Link to="/">Login</Link>
                        <Link to="/home">Home</Link>
                    </nav>
                )} */}
            <Route exact path="/" component={Login} />
            <Route path="/home" render={() => <Home />} />
        </Router>
    );
}

ReactDOM.render(
    <QueryClientProvider client={queryClient}>
        <App />
    </QueryClientProvider>,
    document.getElementById('root')
);


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
