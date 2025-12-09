import { useState } from "react";
import { authApi } from "@/lib/api/auth";

export function useAuth() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [user, setUser] = useState<any>(null);


  //        LOGIN

  async function login(data: { email: string; password: string }) {
    try {
      setLoading(true);
      setError(null);

      const res = await authApi.login(data);

      
      localStorage.setItem("token", res.token);
      localStorage.setItem("tallerId", res.id);
      localStorage.setItem("email", res.email);
      localStorage.setItem("nombre", res.nombre);

      setUser({
        id: res.id,
        nombre: res.nombre,
        email: res.email,
        token: res.token,
      });

      return res;
    } catch (err: any) {
      throw new Error(err.message || "Error al iniciar sesión");
    } finally {
      setLoading(false);
    }
  }


  //        REGISTER 

  const register = async (data: {
    nombre: string;
    email: string;
    password: string;
    confirmPassword: string;
  }) => {
    try {
      setLoading(true);
      setError(null);

      const res = await authApi.register({
        nombre: data.nombre,
        email: data.email,
        password: data.password,
        confirmPassword: data.confirmPassword,
      });

      
      localStorage.setItem("token", res.token);
      localStorage.setItem("tallerId", res.id);
      localStorage.setItem("email", res.email);
      localStorage.setItem("nombre", res.nombre);

      setUser({
        id: res.id,
        nombre: res.nombre,
        email: res.email,
        token: res.token,
      });

      return res;
    } catch (err: any) {
      setError(err.message || "Error al registrarse");
      throw err;
    } finally {
      setLoading(false);
    }
  };


  //        LOGOUT
 
  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("tallerId");
    localStorage.removeItem("email");
    localStorage.removeItem("nombre");
    localStorage.removeItem("user");

    setUser(null);
  };

  return {
    loading,
    error,
    user,
    login,
    register,
    logout,
  };
}
